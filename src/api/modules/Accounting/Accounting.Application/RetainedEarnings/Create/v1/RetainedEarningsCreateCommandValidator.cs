namespace Accounting.Application.RetainedEarnings.Create.v1;

/// <summary>
/// Validator for retained earnings creation command.
/// </summary>
public class RetainedEarningsCreateCommandValidator : AbstractValidator<RetainedEarningsCreateCommand>
{
    public RetainedEarningsCreateCommandValidator()
    {
        RuleFor(x => x.FiscalYear)
            .GreaterThanOrEqualTo(1900).WithMessage("Fiscal year must be 1900 or later")
            .LessThanOrEqualTo(2100).WithMessage("Fiscal year must be 2100 or earlier");

        RuleFor(x => x.FiscalYearStartDate)
            .NotEmpty().WithMessage("Fiscal year start date is required")
            .LessThan(x => x.FiscalYearEndDate).WithMessage("Fiscal year start date must be before end date");

        RuleFor(x => x.FiscalYearEndDate)
            .NotEmpty().WithMessage("Fiscal year end date is required")
            .GreaterThan(x => x.FiscalYearStartDate).WithMessage("Fiscal year end date must be after start date");

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

