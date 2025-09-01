using Accounting.Application.GeneralLedger.Commands.PostJournalEntry.v1;
using FluentValidation;

namespace Accounting.Application.GeneralLedger.Commands.PostJournalEntry.v1;

public class PostJournalEntryCommandValidator : AbstractValidator<PostJournalEntryCommand>
{
    public PostJournalEntryCommandValidator()
    {
        RuleFor(x => x.JournalEntryId)
            .NotEmpty()
            .WithMessage("Journal Entry ID is required");

        RuleFor(x => x.PostingDate)
            .NotEmpty()
            .WithMessage("Posting date is required")
            .LessThanOrEqualTo(DateTime.Today.AddDays(1))
            .WithMessage("Posting date cannot be in the future");

        RuleFor(x => x.PostingReference)
            .MaximumLength(50)
            .WithMessage("Posting reference cannot exceed 50 characters");

        RuleFor(x => x.PostingNotes)
            .MaximumLength(500)
            .WithMessage("Posting notes cannot exceed 500 characters");
    }
}
