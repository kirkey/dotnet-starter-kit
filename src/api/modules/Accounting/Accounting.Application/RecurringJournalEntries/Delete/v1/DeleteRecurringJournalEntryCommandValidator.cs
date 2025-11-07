namespace Accounting.Application.RecurringJournalEntries.Delete.v1;

public sealed class DeleteRecurringJournalEntryCommandValidator : AbstractValidator<DeleteRecurringJournalEntryCommand>
{
    public DeleteRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Recurring journal entry ID is required.");
    }
}

