namespace Accounting.Application.RecurringJournalEntries.Generate.v1;

public sealed class GenerateRecurringJournalEntryCommandValidator : AbstractValidator<GenerateRecurringJournalEntryCommand>
{
    public GenerateRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Recurring journal entry ID is required.");
    }
}

