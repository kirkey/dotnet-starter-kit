namespace Accounting.Application.RecurringJournalEntries.Reactivate.v1;

public sealed class ReactivateRecurringJournalEntryCommandValidator : AbstractValidator<ReactivateRecurringJournalEntryCommand>
{
    public ReactivateRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Recurring journal entry ID is required.");
    }
}

