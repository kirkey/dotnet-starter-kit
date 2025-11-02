namespace Accounting.Application.JournalEntries.Reverse;

/// <summary>
/// Validator for reverse journal entry command.
/// Ensures all required fields are provided.
/// </summary>
public class ReverseJournalEntryValidator : AbstractValidator<ReverseJournalEntryCommand>
{
    public ReverseJournalEntryValidator()
    {
        RuleFor(x => x.JournalEntryId)
            .NotEmpty()
            .WithMessage("Journal Entry ID is required.");

        RuleFor(x => x.ReversalDate)
            .NotEmpty()
            .WithMessage("Reversal date is required.");

        RuleFor(x => x.ReversalReason)
            .NotEmpty()
            .WithMessage("Reversal reason is required.")
            .MaximumLength(500)
            .WithMessage("Reversal reason must not exceed 500 characters.");
    }
}

