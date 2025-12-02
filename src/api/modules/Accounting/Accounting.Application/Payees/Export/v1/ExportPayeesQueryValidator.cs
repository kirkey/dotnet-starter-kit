namespace Accounting.Application.Payees.Export.v1;

/// <summary>
/// Validator for the ExportPayeesQuery to ensure valid filter parameters.
/// Validates expense account codes, search terms, and logical filter combinations.
/// </summary>
public sealed class ExportPayeesQueryValidator : AbstractValidator<ExportPayeesQuery>
{
    /// <summary>
    /// Initializes validation rules for Payees export query.
    /// Enforces strict parameter validation to ensure meaningful export results.
    /// </summary>
    public ExportPayeesQueryValidator()
    {
        RuleFor(x => x.ExpenseAccountCode)
            .MaximumLength(64)
            .When(x => !string.IsNullOrWhiteSpace(x.ExpenseAccountCode))
            .WithMessage("Expense account code cannot exceed 50 characters");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(128)
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm))
            .WithMessage("Search term cannot exceed 100 characters");
    }
}
