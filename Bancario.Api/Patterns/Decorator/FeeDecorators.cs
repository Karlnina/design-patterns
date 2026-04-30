using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.Decorator;

/// <summary>
/// Decorator: compone dinámicamente cargos adicionales sobre una tarifa base.
/// Cada decorador añade su propio cargo sin modificar los demás.
/// </summary>
public interface IFeeComponent
{
    decimal Compute(BankTransaction transaction, decimal runningTotal);
}

public sealed class BaseFeeComponent : IFeeComponent
{
    public decimal Compute(BankTransaction transaction, decimal runningTotal) =>
        runningTotal + 1m;
}

public abstract class FeeDecorator : IFeeComponent
{
    private readonly IFeeComponent _inner;

    protected FeeDecorator(IFeeComponent inner) => _inner = inner;

    public decimal Compute(BankTransaction transaction, decimal runningTotal)
    {
        var baseTotal = _inner.Compute(transaction, runningTotal);
        return AddFee(transaction, baseTotal);
    }

    protected abstract decimal AddFee(BankTransaction transaction, decimal current);
}

public sealed class TaxFeeDecorator : FeeDecorator
{
    public TaxFeeDecorator(IFeeComponent inner) : base(inner) { }

    protected override decimal AddFee(BankTransaction transaction, decimal current) =>
        current + decimal.Round(transaction.Amount * 0.01m, 2, MidpointRounding.AwayFromZero);
}

public sealed class InsuranceFeeDecorator : FeeDecorator
{
    public InsuranceFeeDecorator(IFeeComponent inner) : base(inner) { }

    protected override decimal AddFee(BankTransaction transaction, decimal current) =>
        transaction.IncludeInsurance ? current + 3m : current;
}

public sealed class PriorityFeeDecorator : FeeDecorator
{
    public PriorityFeeDecorator(IFeeComponent inner) : base(inner) { }

    protected override decimal AddFee(BankTransaction transaction, decimal current) =>
        string.Equals(transaction.Priority, "fast", StringComparison.OrdinalIgnoreCase)
            ? current + 5m
            : current;
}
