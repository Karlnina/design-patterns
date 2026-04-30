using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.Observer;

/// <summary>
/// Observer: emisor (TransactionSubject) notifica a suscriptores sin conocerlos directamente.
/// Añadir un nuevo canal de notificación solo requiere implementar ITransactionObserver.
/// </summary>
public interface ITransactionObserver
{
    string Name { get; }
    string OnCompleted(TransactionCompletedEvent @event);
}

public sealed class EmailObserver : ITransactionObserver
{
    public string Name => "Observer/Email";
    public string OnCompleted(TransactionCompletedEvent e) =>
        $"Email sent for transaction {e.TransactionId}";
}

public sealed class AuditObserver : ITransactionObserver
{
    public string Name => "Observer/Audit";
    public string OnCompleted(TransactionCompletedEvent e) =>
        $"Audit event stored for transaction {e.TransactionId}";
}

public sealed class TransactionSubject
{
    private readonly IEnumerable<ITransactionObserver> _observers;

    public TransactionSubject(IEnumerable<ITransactionObserver> observers) =>
        _observers = observers;

    public List<string> Notify(TransactionCompletedEvent @event, List<string> trace)
    {
        var results = new List<string>();
        foreach (var obs in _observers)
        {
            trace.Add(obs.Name);
            results.Add(obs.OnCompleted(@event));
        }
        return results;
    }
}
