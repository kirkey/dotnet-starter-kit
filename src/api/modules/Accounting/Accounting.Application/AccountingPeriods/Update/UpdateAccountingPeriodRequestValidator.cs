namespace Accounting.Application.AccountingPeriods.Update;

public class UpdateAccountingPeriodRequestValidator : AbstractValidator<UpdateAccountingPeriodRequest>
{
    private static readonly HashSet<string> AllowedPeriodTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Monthly",
        "Quarterly",
        "Yearly",
        "Annual",
    };

    public UpdateAccountingPeriodRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.FiscalYear)
            .GreaterThan(1899)
            .LessThanOrEqualTo(2100)
            .When(x => x.FiscalYear.HasValue);

        RuleFor(x => x.PeriodType)
            .Must(pt => string.IsNullOrWhiteSpace(pt) || (!string.IsNullOrWhiteSpace(pt) && pt!.Trim().Length <= 16))
            .WithMessage("PeriodType cannot exceed 16 characters (trimmed).")
            .Must(pt => string.IsNullOrWhiteSpace(pt) || AllowedPeriodTypes.Contains(pt!.Trim()))
            .WithMessage(x => $"PeriodType must be one of: {string.Join(", ", AllowedPeriodTypes)}")
            .When(x => !string.IsNullOrEmpty(x.PeriodType));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
