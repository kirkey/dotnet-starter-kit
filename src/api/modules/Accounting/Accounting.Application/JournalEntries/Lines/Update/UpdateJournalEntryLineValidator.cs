namespace Accounting.Application.JournalEntries.Lines.Update;

/// <summary>
/// Validator for UpdateJournalEntryLineCommand.
/// </summary>
public sealed class UpdateJournalEntryLineValidator : CustomValidator<UpdateJournalEntryLineCommand>
{
    public UpdateJournalEntryLineValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Journal Entry Line ID is required.");

        RuleFor(x => x.DebitAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.DebitAmount.HasValue)
            .WithMessage("Debit amount must be non-negative.");

        RuleFor(x => x.CreditAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.CreditAmount.HasValue)
            .WithMessage("Credit amount must be non-negative.");

        RuleFor(x => x.Memo)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Memo))
            .WithMessage("Memo cannot exceed 500 characters.");

        RuleFor(x => x.Reference)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Reference))
            .WithMessage("Reference cannot exceed 100 characters.");
    }
}

