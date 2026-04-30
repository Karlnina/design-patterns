namespace Bancario.Api.Patterns.Singleton;

/// <summary>
/// Singleton: instancia única y compartida de tasas de cambio.
/// Usa Lazy&lt;T&gt; para inicialización thread-safe sin locks explícitos.
/// </summary>
public sealed class ExchangeRateRegistry
{
    private static readonly Lazy<ExchangeRateRegistry> LazyInstance =
        new(() => new ExchangeRateRegistry());

    private readonly Dictionary<string, decimal> _usdRates = new(StringComparer.OrdinalIgnoreCase)
    {
        ["USD"] = 1m,
        ["EUR"] = 1.08m,
        ["MXN"] = 0.059m,
        ["COP"] = 0.00026m
    };

    public static ExchangeRateRegistry Instance => LazyInstance.Value;

    private ExchangeRateRegistry() { }

    public decimal ToUsd(string currency, decimal amount)
    {
        if (!_usdRates.TryGetValue(currency, out var rate))
            throw new InvalidOperationException($"Currency not supported: {currency}");

        return decimal.Round(amount * rate, 2, MidpointRounding.AwayFromZero);
    }
}
