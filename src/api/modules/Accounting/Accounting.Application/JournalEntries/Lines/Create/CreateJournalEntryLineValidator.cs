namespace Accounting.Application.JournalEntries.Lines.Create;

/// <summary>
/// Validator for CreateJournalEntryLineCommand.
/// </summary>
public sealed class CreateJournalEntryLineValidator : AbstractValidator<CreateJournalEntryLineCommand>
{
    public CreateJournalEntryLineValidator()
    {
        RuleFor(x => x.JournalEntryId)
            .NotEmpty()
            .WithMessage("Journal Entry ID is required.");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(x => x.DebitAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Debit amount must be non-negative.");

        RuleFor(x => x.CreditAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Credit amount must be non-negative.");

        RuleFor(x => x)
            .Must(x => x.DebitAmount != 0 || x.CreditAmount != 0)
            .WithMessage("Either debit or credit amount must be non-zero.");

        RuleFor(x => x)
            .Must(x => !(x.DebitAmount != 0 && x.CreditAmount != 0))
            .WithMessage("Cannot have both debit and credit amounts non-zero.");

        RuleFor(x => x.Memo)
            .MaximumLength(512)
            .When(x => !string.IsNullOrEmpty(x.Memo))
            .WithMessage("Memo cannot exceed 500 characters.");

        RuleFor(x => x.Reference)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.Reference))
            .WithMessage("Reference cannot exceed 100 characters.");
    }
}
