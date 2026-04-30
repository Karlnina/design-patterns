using System.Collections;
using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.Iterator;

/// <summary>
/// Iterator: TransactionBatch controla el recorrido interno de la colección.
/// El consumidor itera con foreach sin conocer la estructura de almacenamiento.
/// </summary>
public sealed class TransactionBatch : IEnumerable<TransactionRequest>
{
    private readonly IReadOnlyList<TransactionRequest> _transactions;

    public TransactionBatch(IEnumerable<TransactionRequest> transactions) =>
        _transactions = transactions.ToList();

    public IEnumerator<TransactionRequest> GetEnumerator()
    {
        for (var i = 0; i < _transactions.Count; i++)
            yield return _transactions[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
