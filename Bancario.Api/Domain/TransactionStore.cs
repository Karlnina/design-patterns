namespace Bancario.Api.Domain;

public record StoredTransaction(
    string TransactionId,
    DateTimeOffset SavedAt,
    TransactionRequest Request,
    TransactionResponse Response);

public interface ITransactionStore
{
    void Save(TransactionRequest request, TransactionResponse response);
    IReadOnlyList<StoredTransaction> GetAll();
    StoredTransaction? GetById(string transactionId);
}

public sealed class InMemoryTransactionStore : ITransactionStore
{
    private readonly List<StoredTransaction> _items = [];
    private readonly object _lock = new();

    public void Save(TransactionRequest request, TransactionResponse response)
    {
        var item = new StoredTransaction(
            response.TransactionId,
            DateTimeOffset.UtcNow,
            request,
            response);

        lock (_lock)
        {
            _items.Add(item);
        }
    }

    public IReadOnlyList<StoredTransaction> GetAll()
    {
        lock (_lock)
        {
            return _items
                .OrderByDescending(x => x.SavedAt)
                .ToList();
        }
    }

    public StoredTransaction? GetById(string transactionId)
    {
        lock (_lock)
        {
            return _items.FirstOrDefault(x =>
                string.Equals(x.TransactionId, transactionId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
