using Bancario.Api.Domain;
using Bancario.Api.Patterns.TemplateMethod;

namespace Bancario.Api.Patterns.Command;

/// <summary>
/// Command: encapsula cada solicitud como un objeto ejecutable.
/// CommandBus desacopla al invocador del ejecutor, habilitando extensiones como colas o reintentos.
/// </summary>
public interface ICommand<out TResult>
{
    TResult Execute();
}

public sealed class ProcessTransactionCommand : ICommand<TransactionResponse>
{
    private readonly TransactionOrchestrator _orchestrator;
    private readonly TransactionRequest _request;
    private readonly bool _includeDebug;

    public ProcessTransactionCommand(
        TransactionOrchestrator orchestrator,
        TransactionRequest request,
        bool includeDebug)
    {
        _orchestrator = orchestrator;
        _request = request;
        _includeDebug = includeDebug;
    }

    public TransactionResponse Execute() =>
        _orchestrator.Process(_request, _includeDebug);
}

public sealed class CommandBus
{
    public TResult Execute<TResult>(ICommand<TResult> command) => command.Execute();
}
