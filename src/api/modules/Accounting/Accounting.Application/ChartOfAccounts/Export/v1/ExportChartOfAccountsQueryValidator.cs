namespace Accounting.Application.ChartOfAccounts.Export.v1;

/// <summary>
/// Validator for the ExportChartOfAccountsQuery to ensure valid filter parameters.
/// Validates account type, USOA category, and logical filter combinations.
/// </summary>
public sealed class ExportChartOfAccountsQueryValidator : AbstractValidator<ExportChartOfAccountsQuery>
{
    /// <summary>
    /// Initializes validation rules for Chart of Accounts export query.
    /// Enforces strict parameter validation to ensure meaningful export results.
    /// </summary>
    public ExportChartOfAccountsQueryValidator()
    {
        RuleFor(x => x.AccountType)
            .Must(BeValidAccountType)
            .When(x => !string.IsNullOrWhiteSpace(x.AccountType))
            .WithMessage("Account type must be one of: Asset, Liability, Equity, Revenue, Expense");

        RuleFor(x => x.UsoaCategory)
            .Must(BeValidUsoaCategory)
            .When(x => !string.IsNullOrWhiteSpace(x.UsoaCategory))
            .WithMessage("USOA category must be one of: Production, Transmission, Distribution, Customer, Administrative, General");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(128)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("Search term cannot exceed 100 characters");

        RuleFor(x => x)
            .Must(x => !(x.OnlyControlAccounts && x.OnlyDetailAccounts))
            .WithMessage("Cannot filter for both control accounts and detail accounts simultaneously");

        RuleFor(x => x.ParentAccountId)
            .NotEqual(DefaultIdType.Empty)
            .When(x => x.ParentAccountId.HasValue)
            .WithMessage("Parent account ID must be a valid identifier");
    }

    /// <summary>
    /// Validates if the account type is one of the allowed values.
    /// </summary>
    private static bool BeValidAccountType(string? accountType)
    {
        if (string.IsNullOrWhiteSpace(accountType)) return true;
        var validTypes = new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" };
        return validTypes.Contains(accountType, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Validates if the USOA category is valid.
    /// </summary>
    private static bool BeValidUsoaCategory(string? usoaCategory)
    {
        if (string.IsNullOrWhiteSpace(usoaCategory)) return true;
        var validCategories = new[] { "Production", "Transmission", "Distribution", "Customer", "Administrative", "General" };
        return validCategories.Contains(usoaCategory, StringComparer.OrdinalIgnoreCase);
    }
}
