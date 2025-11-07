namespace Accounting.Application.RecurringJournalEntries.Approve.v1;

public sealed class ApproveRecurringJournalEntryCommandValidator : AbstractValidator<ApproveRecurringJournalEntryCommand>
{
    public ApproveRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Recurring journal entry ID is required.");
        RuleFor(x => x.ApprovedBy).NotEmpty().WithMessage("Approver information is required.")
            .MaximumLength(200).WithMessage("Approver information must not exceed 200 characters.");
    }
}

