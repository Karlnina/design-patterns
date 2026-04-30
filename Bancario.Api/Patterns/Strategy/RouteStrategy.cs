using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.Strategy;

/// <summary>
/// Strategy: encapsula algoritmos de enrutamiento intercambiables.
/// El cliente trabaja con IRouteStrategy y no conoce la implementación concreta elegida.
/// </summary>
public interface IRouteStrategy
{
    string Name { get; }
    decimal RoutingCost(BankTransaction transaction);
}

public sealed class FastRouteStrategy : IRouteStrategy
{
    public string Name => "Strategy/FastRoute";

    public decimal RoutingCost(BankTransaction transaction) =>
        transaction.IsInternational ? 12.5m : 7.5m;
}

public sealed class EconomicRouteStrategy : IRouteStrategy
{
    public string Name => "Strategy/EconomicRoute";

    public decimal RoutingCost(BankTransaction transaction) =>
        transaction.IsInternational ? 6m : 2.5m;
}

public sealed class RouteStrategyResolver
{
    private readonly IEnumerable<IRouteStrategy> _strategies;

    public RouteStrategyResolver(IEnumerable<IRouteStrategy> strategies) => _strategies = strategies;

    public IRouteStrategy Resolve(BankTransaction transaction) =>
        string.Equals(transaction.Priority, "fast", StringComparison.OrdinalIgnoreCase)
            ? _strategies.OfType<FastRouteStrategy>().First()
            : _strategies.OfType<EconomicRouteStrategy>().First();
}