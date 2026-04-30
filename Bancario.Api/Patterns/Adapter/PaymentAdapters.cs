using System.Globalization;
using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.Adapter;

/// <summary>
/// Adapter: convierte la interfaz de gateways incompatibles a IPaymentAdapter.
/// El pipeline nunca toca el API específico de cada banco; solo habla con IPaymentAdapter.
/// </summary>
public interface IPaymentAdapter
{
    string Name { get; }
    string Charge(BankTransaction transaction, decimal amount);
}

// --- Sistema legado (interfaz propia, no compatible) ---
public sealed class LegacyCoreBanking
{
    public string ExecuteLegacyTransfer(string source, string target, string amountText) =>
        $"LEGACY_OK:{source}:{target}:{amountText}";
}

public sealed class LegacyCoreBankingAdapter : IPaymentAdapter
{
    private readonly LegacyCoreBanking _legacy = new();
    public string Name => "Adapter/LegacyCore";

    public string Charge(BankTransaction transaction, decimal amount)
    {
        var text = amount.ToString("F2", CultureInfo.InvariantCulture);
        return _legacy.ExecuteLegacyTransfer(transaction.FromAccount, transaction.ToAccount, text);
    }
}

// --- Sistema moderno (interfaz propia, tampoco compatible) ---
public sealed class ModernBankingApi
{
    public string PostTransfer(string source, string target, decimal amount) =>
        $"MODERN_OK:{source}:{target}:{amount:F2}";
}

public sealed class ModernBankingApiAdapter : IPaymentAdapter
{
    private readonly ModernBankingApi _modern = new();
    public string Name => "Adapter/ModernApi";

    public string Charge(BankTransaction transaction, decimal amount) =>
        _modern.PostTransfer(transaction.FromAccount, transaction.ToAccount, amount);
}

public sealed class PaymentAdapterSelector
{
    private readonly IEnumerable<IPaymentAdapter> _adapters;

    public PaymentAdapterSelector(IEnumerable<IPaymentAdapter> adapters) => _adapters = adapters;

    public IPaymentAdapter Select(BankTransaction transaction) =>
        string.Equals(transaction.Gateway, "legacy", StringComparison.OrdinalIgnoreCase)
            ? _adapters.OfType<LegacyCoreBankingAdapter>().First()
            : _adapters.OfType<ModernBankingApiAdapter>().First();
}
