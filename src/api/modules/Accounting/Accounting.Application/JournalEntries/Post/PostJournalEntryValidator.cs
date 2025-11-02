namespace Accounting.Application.JournalEntries.Post;

/// <summary>
/// Validator for post journal entry command.
/// Ensures the journal entry ID is valid.
/// </summary>
public class PostJournalEntryValidator : AbstractValidator<PostJournalEntryCommand>
{
    public PostJournalEntryValidator()
    {
        RuleFor(x => x.JournalEntryId)
            .NotEmpty()
            .WithMessage("Journal Entry ID is required.");
    }
}
