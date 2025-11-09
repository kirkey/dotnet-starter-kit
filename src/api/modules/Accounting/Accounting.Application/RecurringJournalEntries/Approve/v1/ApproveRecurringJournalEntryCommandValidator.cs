namespace Accounting.Application.RecurringJournalEntries.Approve.v1;

/// <summary>
/// Validator for approving a recurring journal entry template.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveRecurringJournalEntryCommandValidator : AbstractValidator<ApproveRecurringJournalEntryCommand>
{
    public ApproveRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Recurring journal entry ID is required.");
    }
}
