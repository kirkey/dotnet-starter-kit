namespace Accounting.Application.RecurringJournalEntries.Suspend.v1;

public sealed class SuspendRecurringJournalEntryCommandValidator : AbstractValidator<SuspendRecurringJournalEntryCommand>
{
    public SuspendRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Recurring journal entry ID is required.");
        RuleFor(x => x.Reason).MaximumLength(500).WithMessage("Reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));
    }
}

