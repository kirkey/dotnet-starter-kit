namespace Accounting.Application.AccountingPeriods.Commands.CloseAccountingPeriod.v1;

/// <summary>
/// Validator for <see cref="CloseAccountingPeriodCommand"/> ensuring required fields and reasonable constraints.
/// </summary>
public class CloseAccountingPeriodCommandValidator : AbstractValidator<CloseAccountingPeriodCommand>
{
    public CloseAccountingPeriodCommandValidator()
    {
        RuleFor(x => x.AccountingPeriodId)
            .NotEmpty()
            .WithMessage("Accounting Period ID is required");

        RuleFor(x => x.ClosingDate)
            .NotEmpty()
            .WithMessage("Closing date is required")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Closing date cannot be in the future");

        RuleFor(x => x.ClosingNotes)
            .MaximumLength(500)
            .WithMessage("Closing notes cannot exceed 500 characters");
    }
}
