namespace Accounting.Application.JournalEntries.Create;

public class CreateJournalEntryRequestValidator : AbstractValidator<CreateJournalEntryCommand>
{
    public CreateJournalEntryRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required.");

        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .WithMessage("Reference number is required.")
            .MaximumLength(32)
            .WithMessage("Reference number cannot exceed 32 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Source)
            .NotEmpty()
            .WithMessage("Source is required.")
            .MaximumLength(64)
            .WithMessage("Source cannot exceed 64 characters.");

        RuleFor(x => x.OriginalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Original amount must be non-negative.");

        RuleFor(x => x.Lines)
            .NotNull()
            .WithMessage("Lines are required.")
            .Must(lines => lines is { Count: >= 2 })
            .WithMessage("At least 2 lines are required for a balanced journal entry.");

        When(x => x.Lines != null, () =>
        {
            RuleForEach(x => x.Lines!)
                .ChildRules(line =>
            {
                line.RuleFor(l => l.AccountId)
                    .NotEmpty()
                    .WithMessage("Account ID is required for each line.");

                line.RuleFor(l => l.DebitAmount)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Debit amount must be non-negative.");

                line.RuleFor(l => l.CreditAmount)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Credit amount must be non-negative.");

                line.RuleFor(l => l)
                    .Must(l => l.DebitAmount > 0 || l.CreditAmount > 0)
                    .WithMessage("Each line must have either a debit or credit amount.");

                line.RuleFor(l => l)
                    .Must(l => !(l.DebitAmount > 0 && l.CreditAmount > 0))
                    .WithMessage("A line cannot have both debit and credit amounts.");

                line.RuleFor(l => l.Description)
                    .MaximumLength(500)
                    .When(l => !string.IsNullOrEmpty(l.Description))
                    .WithMessage("Line description cannot exceed 500 characters.");

                line.RuleFor(l => l.Reference)
                    .MaximumLength(100)
                    .When(l => !string.IsNullOrEmpty(l.Reference))
                    .WithMessage("Line reference cannot exceed 100 characters.");
            });
        });

        RuleFor(x => x.Lines)
            .Must(BeBalanced)
            .When(x => x.Lines is { Count: >= 2 })
            .WithMessage("The journal entry must be balanced (total debits must equal total credits).");
    }

    /// <summary>
    /// Validates that the total debits equal the total credits.
    /// </summary>
    private static bool BeBalanced(List<JournalEntryLineDto>? lines)
    {
        if (lines == null || lines.Count == 0)
            return true;

        var totalDebits = lines.Sum(l => l.DebitAmount);
        var totalCredits = lines.Sum(l => l.CreditAmount);

        // Allow a small tolerance for rounding errors
        return Math.Abs(totalDebits - totalCredits) < 0.01m;
    }
}
