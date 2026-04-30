using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.AbstractFactory;

/// <summary>
/// Abstract Factory: garantiza que recibo y auditoría sean coherentes entre sí por región.
/// El cliente obtiene una familia de documentos sin conocer las clases concretas.
/// </summary>
public interface IReceiptDocument
{
    string Build(BankTransaction transaction);
}

public interface IAuditDocument
{
    string Build(BankTransaction transaction);
}

public interface IRegionalDocumentFactory
{
    bool Supports(string region);
    IReceiptDocument CreateReceipt();
    IAuditDocument CreateAudit();
}

// --- Familia LATAM ---
public sealed class LatamReceiptDocument : IReceiptDocument
{
    public string Build(BankTransaction t) => $"REC-LATAM-{t.Id}";
}

public sealed class LatamAuditDocument : IAuditDocument
{
    public string Build(BankTransaction t) => $"AUD-LATAM-{t.Id}";
}

public sealed class LatamDocumentFactory : IRegionalDocumentFactory
{
    public bool Supports(string region) =>
        string.Equals(region, "LATAM", StringComparison.OrdinalIgnoreCase);
    public IReceiptDocument CreateReceipt() => new LatamReceiptDocument();
    public IAuditDocument CreateAudit() => new LatamAuditDocument();
}

// --- Familia Global ---
public sealed class GlobalReceiptDocument : IReceiptDocument
{
    public string Build(BankTransaction t) => $"REC-GLOBAL-{t.Id}";
}

public sealed class GlobalAuditDocument : IAuditDocument
{
    public string Build(BankTransaction t) => $"AUD-GLOBAL-{t.Id}";
}

public sealed class GlobalDocumentFactory : IRegionalDocumentFactory
{
    public bool Supports(string region) =>
        !string.Equals(region, "LATAM", StringComparison.OrdinalIgnoreCase);
    public IReceiptDocument CreateReceipt() => new GlobalReceiptDocument();
    public IAuditDocument CreateAudit() => new GlobalAuditDocument();
}

public sealed class DocumentFactoryResolver
{
    private readonly IEnumerable<IRegionalDocumentFactory> _factories;

    public DocumentFactoryResolver(IEnumerable<IRegionalDocumentFactory> factories) =>
        _factories = factories;

    public IRegionalDocumentFactory Resolve(BankTransaction transaction) =>
        _factories.First(f => f.Supports(transaction.Region));
}
