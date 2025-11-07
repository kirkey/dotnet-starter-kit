namespace Accounting.Application.RecurringJournalEntries.Update.v1;

public sealed class UpdateRecurringJournalEntryCommandValidator : AbstractValidator<UpdateRecurringJournalEntryCommand>
{
    public UpdateRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Recurring journal entry ID is required.");
        
        RuleFor(x => x.Description).MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.")
            .When(x => x.Amount.HasValue);
        
        RuleFor(x => x.Memo).MaximumLength(500).WithMessage("Memo must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Memo));
        
        RuleFor(x => x.Notes).MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        
        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Description) || x.Amount.HasValue || 
                                  x.EndDate.HasValue || !string.IsNullOrWhiteSpace(x.Memo) || !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("At least one field must be provided for update.");
    }
}

