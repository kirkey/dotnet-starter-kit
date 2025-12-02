namespace Accounting.Application.JournalEntries.Update;

public class UpdateJournalEntryValidator : AbstractValidator<UpdateJournalEntryCommand>
{
    public UpdateJournalEntryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        
        When(x => x.Date.HasValue, () =>
        {
            RuleFor(x => x.Date!.Value)
                .NotEmpty()
                .WithMessage("Date must be provided");
        });

        When(x => !string.IsNullOrEmpty(x.ReferenceNumber), () =>
        {
            RuleFor(x => x.ReferenceNumber)
                .MaximumLength(64)
                .WithMessage("Reference number cannot exceed 50 characters");
        });

        When(x => !string.IsNullOrEmpty(x.Source), () =>
        {
            RuleFor(x => x.Source)
                .MaximumLength(128)
                .WithMessage("Source cannot exceed 100 characters");
        });

        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(512)
                .WithMessage("Description cannot exceed 500 characters");
        });
    }
}
