using Bancario.Api.Domain;
using Bancario.Api.Patterns.AbstractFactory;
using Bancario.Api.Patterns.Adapter;
using Bancario.Api.Patterns.Builder;
using Bancario.Api.Patterns.Command;
using Bancario.Api.Patterns.Composite;
using Bancario.Api.Patterns.Decorator;
using Bancario.Api.Patterns.FactoryMethod;
using Bancario.Api.Patterns.Iterator;
using Bancario.Api.Patterns.Observer;
using Bancario.Api.Patterns.Proxy;
using Bancario.Api.Patterns.Singleton;
using Bancario.Api.Patterns.Strategy;
using Bancario.Api.Patterns.TemplateMethod;

namespace Bancario.Api;

public static class BankingModule
{
    public static IServiceCollection AddBankingModule(this IServiceCollection services)
    {
        // Singleton
        services.AddSingleton(_ => ExchangeRateRegistry.Instance);
        services.AddSingleton<CommandBus>();
        services.AddSingleton<ITransactionStore, InMemoryTransactionStore>();

        // Builder
        services.AddScoped<ITransactionBuilder, TransactionBuilder>();

        // Factory Method
        services.AddScoped<IProcessorCreator, DomesticProcessorCreator>();
        services.AddScoped<IProcessorCreator, InternationalProcessorCreator>();
        services.AddScoped<ProcessorFactory>();

        // Strategy
        services.AddScoped<IRouteStrategy, FastRouteStrategy>();
        services.AddScoped<IRouteStrategy, EconomicRouteStrategy>();
        services.AddScoped<RouteStrategyResolver>();

        // Adapter
        services.AddScoped<IPaymentAdapter, LegacyCoreBankingAdapter>();
        services.AddScoped<IPaymentAdapter, ModernBankingApiAdapter>();
        services.AddScoped<PaymentAdapterSelector>();

        // Composite
        services.AddScoped<IValidationRule, PositiveAmountRule>();
        services.AddScoped<IValidationRule, DistinctAccountsRule>();
        services.AddScoped<IValidationRule, SupportedCurrencyRule>();
        services.AddScoped<ValidationComposite>();

        // Proxy
        services.AddScoped<AccountService>();
        services.AddScoped<IAccountService>(sp =>
            new AccountServiceProxy(sp.GetRequiredService<AccountService>()));

        // Observer
        services.AddScoped<ITransactionObserver, EmailObserver>();
        services.AddScoped<ITransactionObserver, AuditObserver>();
        services.AddScoped<TransactionSubject>();

        // Abstract Factory
        services.AddScoped<IRegionalDocumentFactory, LatamDocumentFactory>();
        services.AddScoped<IRegionalDocumentFactory, GlobalDocumentFactory>();
        services.AddScoped<DocumentFactoryResolver>();

        // Template Method
        services.AddScoped<TransactionTemplate, TransferTransactionTemplate>();
        services.AddScoped<TransactionOrchestrator>();

        return services;
    }

    public static IEndpointRouteBuilder MapBankingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapGet("/patterns", () => Results.Ok(PatternCatalog.All));

        group.MapGet("/transactions", (ITransactionStore store) =>
            Results.Ok(store.GetAll()));

        group.MapGet("/transactions/{id}", (string id, ITransactionStore store) =>
        {
            var item = store.GetById(id);
            return item is null ? Results.NotFound() : Results.Ok(item);
        });

        group.MapPost("/transactions/process",
            (TransactionRequest request, HttpContext http, CommandBus bus, TransactionOrchestrator orchestrator, ITransactionStore store) =>
            {
                var debug = http.Request.Headers.TryGetValue("X-Debug-Patterns", out var v) &&
                            string.Equals(v.ToString(), "true", StringComparison.OrdinalIgnoreCase);
                var result = bus.Execute(new ProcessTransactionCommand(orchestrator, request, debug));
                store.Save(request, result);
                return Results.Ok(result);
            });

        group.MapPost("/transactions/batch/process",
            (TransactionBatchRequest batch, HttpContext http, CommandBus bus, TransactionOrchestrator orchestrator, ITransactionStore store) =>
            {
                var debug = http.Request.Headers.TryGetValue("X-Debug-Patterns", out var v) &&
                            string.Equals(v.ToString(), "true", StringComparison.OrdinalIgnoreCase);
                var responses = new List<TransactionResponse>();
                foreach (var request in new TransactionBatch(batch.Transactions))
                {
                    var result = bus.Execute(new ProcessTransactionCommand(orchestrator, request, debug));
                    responses.Add(result);
                    store.Save(request, result);
                }
                return Results.Ok(new BatchTransactionResponse(responses));
            });

        return app;
    }
}

public static class PatternCatalog
{
    public static readonly string[] All =
    [
        "Factory Method   -> Patterns/FactoryMethod/ProcessorFactory.cs",
        "Builder          -> Patterns/Builder/TransactionBuilder.cs",
        "Singleton        -> Patterns/Singleton/ExchangeRateRegistry.cs",
        "Abstract Factory -> Patterns/AbstractFactory/DocumentFactory.cs",
        "Proxy            -> Patterns/Proxy/AccountService.cs",
        "Adapter          -> Patterns/Adapter/PaymentAdapters.cs",
        "Decorator        -> Patterns/Decorator/FeeDecorators.cs",
        "Composite        -> Patterns/Composite/ValidationComposite.cs",
        "Iterator         -> Patterns/Iterator/TransactionBatch.cs",
        "Command          -> Patterns/Command/CommandBus.cs",
        "Strategy         -> Patterns/Strategy/RouteStrategies.cs",
        "Observer         -> Patterns/Observer/TransactionObservers.cs",
        "Template Method  -> Patterns/TemplateMethod/TransactionTemplate.cs",
        "DI               -> BankingModule.cs (registro + endpoints)"
    ];
}
