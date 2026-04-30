namespace Bancario.Api.Patterns.Proxy;

/// <summary>
/// Proxy: AccountServiceProxy controla el acceso al servicio real de cuentas.
/// Bloquea cuentas restringidas y delega el resto sin que el cliente lo note.
/// </summary>
public interface IAccountService
{
    bool HasFunds(string account, decimal amount);
    void Debit(string account, decimal amount);
    void Credit(string account, decimal amount);
}

public sealed class AccountService : IAccountService
{
    private readonly Dictionary<string, decimal> _balances = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ACC-001"] = 20000m,
        ["ACC-002"] = 7000m,
        ["ACC-003"] = 1500m,
        ["ACC-004"] = 800m
    };

    public bool HasFunds(string account, decimal amount) =>
        _balances.TryGetValue(account, out var current) && current >= amount;

    public void Debit(string account, decimal amount)
    {
        if (!_balances.TryGetValue(account, out var current))
            throw new InvalidOperationException("Source account does not exist");
        _balances[account] = current - amount;
    }

    public void Credit(string account, decimal amount)
    {
        if (!_balances.ContainsKey(account))
            _balances[account] = 0;
        _balances[account] += amount;
    }
}

public sealed class AccountServiceProxy : IAccountService
{
    private readonly IAccountService _inner;

    public AccountServiceProxy(IAccountService inner) => _inner = inner;

    public bool HasFunds(string account, decimal amount)
    {
        if (account.StartsWith("BLOCK", StringComparison.OrdinalIgnoreCase))
            return false;
        return _inner.HasFunds(account, amount);
    }

    public void Debit(string account, decimal amount) => _inner.Debit(account, amount);
    public void Credit(string account, decimal amount) => _inner.Credit(account, amount);
}
