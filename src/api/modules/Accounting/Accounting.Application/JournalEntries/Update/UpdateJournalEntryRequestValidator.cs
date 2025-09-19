namespace Accounting.Application.JournalEntries.Update;

public class UpdateJournalEntryRequestValidator : AbstractValidator<UpdateJournalEntryCommand>
{
    public UpdateJournalEntryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(32)
            .When(x => !string.IsNullOrEmpty(x.ReferenceNumber));

        RuleFor(x => x.Source)
            .MaximumLength(64)
            .When(x => !string.IsNullOrEmpty(x.Source));

        RuleFor(x => x.OriginalAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.OriginalAmount.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
