using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.Builder;

/// <summary>
/// Builder: centraliza la construcción de BankTransaction desde un request externo.
/// Garantiza que el objeto resultante esté completo y normalizado antes de entrar al pipeline.
/// </summary>
public interface ITransactionBuilder
{
    BankTransaction Build(TransactionRequest request);
}

public sealed class TransactionBuilder : ITransactionBuilder
{
    public BankTransaction Build(TransactionRequest request) =>
        new()
        {
            FromAccount = request.FromAccount,
            ToAccount = request.ToAccount,
            Amount = request.Amount,
            Currency = request.Currency.ToUpperInvariant(),
            Region = request.Region,
            Gateway = request.Gateway,
            Priority = request.Priority,
            IncludeInsurance = request.IncludeInsurance,
            IsInternational = request.IsInternational,
            RequestedBy = request.RequestedBy,
            CreatedAt = DateTimeOffset.UtcNow
        };
}
