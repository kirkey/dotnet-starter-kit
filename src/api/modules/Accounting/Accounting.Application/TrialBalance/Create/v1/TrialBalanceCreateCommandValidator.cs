namespace Accounting.Application.TrialBalance.Create.v1;

/// <summary>
/// Validator for TrialBalanceCreateCommand.
/// </summary>
public sealed class TrialBalanceCreateCommandValidator : AbstractValidator<TrialBalanceCreateCommand>
{
    public TrialBalanceCreateCommandValidator()
    {
        RuleFor(x => x.TrialBalanceNumber)
            .NotEmpty()
            .WithMessage("Trial balance number is required.")
            .MaximumLength(50)
            .WithMessage("Trial balance number must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("Trial balance number can only contain letters, numbers, and hyphens.");

        RuleFor(x => x.PeriodId)
            .NotEmpty()
            .WithMessage("Accounting period ID is required.");

        RuleFor(x => x.PeriodStartDate)
            .NotEmpty()
            .WithMessage("Period start date is required.")
            .LessThan(x => x.PeriodEndDate)
            .WithMessage("Period start date must be before period end date.");

        RuleFor(x => x.PeriodEndDate)
            .NotEmpty()
            .WithMessage("Period end date is required.")
            .GreaterThan(x => x.PeriodStartDate)
            .WithMessage("Period end date must be after period start date.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .WithMessage("Notes must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

