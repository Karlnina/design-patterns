using Bancario.Api.Domain;
using Bancario.Api.Patterns.AbstractFactory;
using Bancario.Api.Patterns.Adapter;
using Bancario.Api.Patterns.Builder;
using Bancario.Api.Patterns.Composite;
using Bancario.Api.Patterns.Decorator;
using Bancario.Api.Patterns.FactoryMethod;
using Bancario.Api.Patterns.Observer;
using Bancario.Api.Patterns.Proxy;
using Bancario.Api.Patterns.Singleton;
using Bancario.Api.Patterns.Strategy;

namespace Bancario.Api.Patterns.TemplateMethod;

/// <summary>
/// Template Method: define la secuencia fija del proceso transaccional.
/// Las subclases implementan cada paso; el orden y el contrato son invariables.
/// Colabora con todos los demás patrones: es el pegamento central del pipeline.
/// </summary>
public abstract class TransactionTemplate
{
    public TransactionResponse Execute(TransactionExecutionContext context)
    {
        Validate(context);
        if (!string.Equals(context.Status, "OK", StringComparison.OrdinalIgnoreCase))
            return BuildResponse(context);

        Prepare(context);
        Charge(context);
        GenerateDocuments(context);
        Notify(context);
        return BuildResponse(context);
    }

    protected abstract void Validate(TransactionExecutionContext context);
    protected abstract void Prepare(TransactionExecutionContext context);
    protected abstract void Charge(TransactionExecutionContext context);
    protected abstract void GenerateDocuments(TransactionExecutionContext context);
    protected abstract void Notify(TransactionExecutionContext context);

    private static TransactionResponse BuildResponse(TransactionExecutionContext ctx) =>
        new(ctx.Transaction.Id, ctx.ProcessorName, ctx.RouteName, ctx.GatewayName,
            ctx.Receipt, ctx.Audit, ctx.Transaction.Amount, ctx.AmountInUsd,
            ctx.FinalAmount, ctx.IncludeDebug ? ctx.PatternTrace : null,
            ctx.Status, ctx.Message);
}

public sealed class TransferTransactionTemplate : TransactionTemplate
{
    private readonly ValidationComposite _validation;
    private readonly ProcessorFactory _processorFactory;
    private readonly RouteStrategyResolver _strategyResolver;
    private readonly PaymentAdapterSelector _paymentSelector;
    private readonly IAccountService _accountService;
    private readonly DocumentFactoryResolver _documents;
    private readonly TransactionSubject _subject;
    private readonly ExchangeRateRegistry _rates;

    public TransferTransactionTemplate(
        ValidationComposite validation,
        ProcessorFactory processorFactory,
        RouteStrategyResolver strategyResolver,
        PaymentAdapterSelector paymentSelector,
        IAccountService accountService,
        DocumentFactoryResolver documents,
        TransactionSubject subject,
        ExchangeRateRegistry rates)
    {
        _validation = validation;
        _processorFactory = processorFactory;
        _strategyResolver = strategyResolver;
        _paymentSelector = paymentSelector;
        _accountService = accountService;
        _documents = documents;
        _subject = subject;
        _rates = rates;
    }

    protected override void Validate(TransactionExecutionContext ctx)
    {
        var result = _validation.ValidateAll(ctx.Transaction, ctx.PatternTrace);
        if (!result.IsValid) { ctx.Status = "Rejected"; ctx.Message = result.Message; return; }

        if (!_accountService.HasFunds(ctx.Transaction.FromAccount, ctx.Transaction.Amount))
        {
            ctx.PatternTrace.Add("Proxy/AccountServiceProxy");
            ctx.Status = "Rejected";
            ctx.Message = "Insufficient funds or blocked account";
            return;
        }

        ctx.PatternTrace.Add("Proxy/AccountServiceProxy");
        ctx.Status = "OK";
    }

    protected override void Prepare(TransactionExecutionContext ctx)
    {
        var processor = _processorFactory.Create(ctx.Transaction);
        processor.Process(ctx.Transaction, ctx.PatternTrace);
        ctx.ProcessorName = processor.Name;

        var strategy = _strategyResolver.Resolve(ctx.Transaction);
        ctx.PatternTrace.Add(strategy.Name);
        ctx.RouteName = strategy.Name;

        IFeeComponent fee = new BaseFeeComponent();
        fee = new TaxFeeDecorator(fee);
        fee = new InsuranceFeeDecorator(fee);
        fee = new PriorityFeeDecorator(fee);
        ctx.PatternTrace.Add("Decorator/FeeChain");

        var routingCost = strategy.RoutingCost(ctx.Transaction);
        ctx.FinalAmount = ctx.Transaction.Amount + fee.Compute(ctx.Transaction, routingCost);

        ctx.PatternTrace.Add("Singleton/ExchangeRateRegistry");
        ctx.AmountInUsd = _rates.ToUsd(ctx.Transaction.Currency, ctx.FinalAmount);
    }

    protected override void Charge(TransactionExecutionContext ctx)
    {
        var adapter = _paymentSelector.Select(ctx.Transaction);
        ctx.PatternTrace.Add(adapter.Name);
        ctx.GatewayName = adapter.Name;

        var code = adapter.Charge(ctx.Transaction, ctx.FinalAmount);
        _accountService.Debit(ctx.Transaction.FromAccount, ctx.FinalAmount);
        _accountService.Credit(ctx.Transaction.ToAccount, ctx.Transaction.Amount);
        ctx.Message = $"Transfer completed with code {code}";
    }

    protected override void GenerateDocuments(TransactionExecutionContext ctx)
    {
        var factory = _documents.Resolve(ctx.Transaction);
        ctx.PatternTrace.Add("AbstractFactory/RegionalDocuments");
        ctx.Receipt = factory.CreateReceipt().Build(ctx.Transaction);
        ctx.Audit = factory.CreateAudit().Build(ctx.Transaction);
    }

    protected override void Notify(TransactionExecutionContext ctx)
    {
        var evt = new TransactionCompletedEvent(
            ctx.Transaction.Id, ctx.FinalAmount, ctx.Transaction.Currency);
        _subject.Notify(evt, ctx.PatternTrace);
    }
}

public sealed class TransactionOrchestrator
{
    private readonly ITransactionBuilder _builder;
    private readonly TransactionTemplate _template;

    public TransactionOrchestrator(ITransactionBuilder builder, TransactionTemplate template)
    {
        _builder = builder;
        _template = template;
    }

    public TransactionResponse Process(TransactionRequest request, bool includeDebug)
    {
        var transaction = _builder.Build(request);
        var context = new TransactionExecutionContext { Transaction = transaction, IncludeDebug = includeDebug };
        context.PatternTrace.Add("Builder/TransactionBuilder");
        context.PatternTrace.Add("TemplateMethod/TransferTransactionTemplate");
        return _template.Execute(context);
    }
}
