namespace Bancario.Api.Domain;

public record TransactionRequest(
    string FromAccount,
    string ToAccount,
    decimal Amount,
    string Currency,
    string Region,
    string Gateway,
    string Priority,
    bool IncludeInsurance,
    bool IsInternational,
    string RequestedBy);

public record TransactionBatchRequest(List<TransactionRequest> Transactions);

public record BatchTransactionResponse(List<TransactionResponse> Results);

public record TransactionResponse(
    string TransactionId,
    string Processor,
    string Route,
    string Gateway,
    string Receipt,
    string Audit,
    decimal OriginalAmount,
    decimal AmountInUsd,
    decimal FinalAmount,
    List<string>? PatternTrace,
    string Status,
    string Message);

public sealed class BankTransaction
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
    public required string FromAccount { get; init; }
    public required string ToAccount { get; init; }
    public required decimal Amount { get; init; }
    public required string Currency { get; init; }
    public required string Region { get; init; }
    public required string Gateway { get; init; }
    public required string Priority { get; init; }
    public required bool IncludeInsurance { get; init; }
    public required bool IsInternational { get; init; }
    public required string RequestedBy { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}

public sealed class TransactionExecutionContext
{
    public required BankTransaction Transaction { get; init; }
    public required bool IncludeDebug { get; init; }
    public List<string> PatternTrace { get; } = [];

    public string ProcessorName { get; set; } = string.Empty;
    public string RouteName { get; set; } = string.Empty;
    public string GatewayName { get; set; } = string.Empty;
    public decimal AmountInUsd { get; set; }
    public decimal FinalAmount { get; set; }
    public string Receipt { get; set; } = string.Empty;
    public string Audit { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public string Message { get; set; } = string.Empty;
}

public record TransactionCompletedEvent(string TransactionId, decimal FinalAmount, string Currency);
