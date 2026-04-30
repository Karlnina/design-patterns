using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.FactoryMethod;

/// <summary>
/// Factory Method: delega la creación del procesador correcto a una clase Creator.
/// El orquestador nunca instancia procesadores directamente; los pide a la fábrica.
/// </summary>
public interface ITransactionProcessor
{
    string Name { get; }
    void Process(BankTransaction transaction, List<string> trace);
}

public sealed class DomesticProcessor : ITransactionProcessor
{
    public string Name => "FactoryMethod/DomesticProcessor";
    public void Process(BankTransaction transaction, List<string> trace) => trace.Add(Name);
}

public sealed class InternationalProcessor : ITransactionProcessor
{
    public string Name => "FactoryMethod/InternationalProcessor";
    public void Process(BankTransaction transaction, List<string> trace) => trace.Add(Name);
}

public interface IProcessorCreator
{
    bool CanHandle(BankTransaction transaction);
    ITransactionProcessor CreateProcessor();
}

public sealed class DomesticProcessorCreator : IProcessorCreator
{
    public bool CanHandle(BankTransaction t) => !t.IsInternational;
    public ITransactionProcessor CreateProcessor() => new DomesticProcessor();
}

public sealed class InternationalProcessorCreator : IProcessorCreator
{
    public bool CanHandle(BankTransaction t) => t.IsInternational;
    public ITransactionProcessor CreateProcessor() => new InternationalProcessor();
}

public sealed class ProcessorFactory
{
    private readonly IEnumerable<IProcessorCreator> _creators;

    public ProcessorFactory(IEnumerable<IProcessorCreator> creators) => _creators = creators;

    public ITransactionProcessor Create(BankTransaction transaction)
    {
        var creator = _creators.FirstOrDefault(c => c.CanHandle(transaction))
            ?? throw new InvalidOperationException("No processor creator found");
        return creator.CreateProcessor();
    }
}
