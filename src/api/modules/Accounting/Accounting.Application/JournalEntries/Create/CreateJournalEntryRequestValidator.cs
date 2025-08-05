using FluentValidation;

namespace Accounting.Application.JournalEntries.Create;

public class CreateJournalEntryRequestValidator : AbstractValidator<CreateJournalEntryRequest>
{
    public CreateJournalEntryRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.Source)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.ExchangeRate)
            .GreaterThan(0);

        RuleFor(x => x.OriginalAmount)
            .GreaterThanOrEqualTo(0);
    }
}
