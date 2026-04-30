using Bancario.Api.Domain;

namespace Bancario.Api.Patterns.Composite;

public record ValidationResult(bool IsValid, string Message);

/// <summary>
/// Composite: trata reglas individuales y el conjunto de reglas de la misma forma.
/// El cliente trabaja con IValidationRule sin saber si hay una o muchas reglas dentro.
/// </summary>
public interface IValidationRule
{
    string Name { get; }
    ValidationResult Validate(BankTransaction transaction);
}

public sealed class PositiveAmountRule : IValidationRule
{
    public string Name => "Composite/PositiveAmountRule";

    public ValidationResult Validate(BankTransaction transaction) =>
        transaction.Amount > 0
            ? new ValidationResult(true, "Amount is valid")
            : new ValidationResult(false, "Amount must be greater than zero");
}

public sealed class DistinctAccountsRule : IValidationRule
{
    public string Name => "Composite/DistinctAccountsRule";

    public ValidationResult Validate(BankTransaction transaction) =>
        !string.Equals(transaction.FromAccount, transaction.ToAccount, StringComparison.OrdinalIgnoreCase)
            ? new ValidationResult(true, "Accounts are distinct")
            : new ValidationResult(false, "From and To account cannot be the same");
}

public sealed class SupportedCurrencyRule : IValidationRule
{
    private static readonly string[] Supported = ["USD", "EUR", "MXN", "COP"];
    public string Name => "Composite/SupportedCurrencyRule";

    public ValidationResult Validate(BankTransaction transaction) =>
        Supported.Contains(transaction.Currency, StringComparer.OrdinalIgnoreCase)
            ? new ValidationResult(true, "Currency is supported")
            : new ValidationResult(false, $"Currency {transaction.Currency} is not supported");
}

public sealed class ValidationComposite
{
    private readonly IEnumerable<IValidationRule> _rules;

    public ValidationComposite(IEnumerable<IValidationRule> rules) => _rules = rules;

    public ValidationResult ValidateAll(BankTransaction transaction, List<string> trace)
    {
        foreach (var rule in _rules)
        {
            trace.Add(rule.Name);
            var result = rule.Validate(transaction);
            if (!result.IsValid) return result;
        }
        return new ValidationResult(true, "All validations passed");
    }
}
