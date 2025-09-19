namespace Accounting.Application.AccountingPeriods.Create.v1;

/// <summary>
/// Validator for <see cref="CreateAccountingPeriodCommand"/>. Enforces required fields and domain-friendly constraints
/// such as maximum lengths, valid period types, fiscal year ranges and non-overlapping dates.
/// </summary>
public class CreateAccountingPeriodCommandValidator : AbstractValidator<CreateAccountingPeriodCommand>
{
    private static readonly HashSet<string> AllowedPeriodTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Monthly",
        "Quarterly",
        "Yearly",
        "Annual",
    };

    public CreateAccountingPeriodCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(1024);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.FiscalYear)
            .GreaterThan(1899)
            .LessThanOrEqualTo(2100);

        RuleFor(x => x.PeriodType)
            .NotEmpty()
            .Must(pt => !string.IsNullOrWhiteSpace(pt) && pt.Trim().Length <= 16)
            .WithMessage("PeriodType cannot exceed 16 characters (trimmed).")
            .Must(pt => AllowedPeriodTypes.Contains(pt!.Trim()))
            .WithMessage(x => $"PeriodType must be one of: {string.Join(", ", AllowedPeriodTypes)}");

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
