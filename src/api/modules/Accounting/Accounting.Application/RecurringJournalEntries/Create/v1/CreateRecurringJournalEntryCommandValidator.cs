namespace Accounting.Application.RecurringJournalEntries.Create.v1;

public sealed class CreateRecurringJournalEntryCommandValidator : AbstractValidator<CreateRecurringJournalEntryCommand>
{
    public CreateRecurringJournalEntryCommandValidator()
    {
        RuleFor(x => x.TemplateCode).NotEmpty().WithMessage("Template code is required.")
            .MaximumLength(64).WithMessage("Template code must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("Template code can only contain letters, numbers, hyphens, and underscores.");
        
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.")
            .MaximumLength(512).WithMessage("Description must not exceed 500 characters.");
        
        RuleFor(x => x.Frequency).NotEmpty().WithMessage("Frequency is required.")
            .Must(f => new[] { "Monthly", "Quarterly", "Annually", "Custom" }.Contains(f))
            .WithMessage("Frequency must be one of: Monthly, Quarterly, Annually, Custom.");
        
        RuleFor(x => x.CustomIntervalDays).GreaterThan(0).WithMessage("Custom interval days must be positive.")
            .When(x => x.Frequency == "Custom");
        
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
        
        RuleFor(x => x.DebitAccountId).NotEmpty().WithMessage("Debit account is required.");
        RuleFor(x => x.CreditAccountId).NotEmpty().WithMessage("Credit account is required.");
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required.");
        
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.")
            .When(x => x.EndDate.HasValue);
        
        RuleFor(x => x.Memo).MaximumLength(512).WithMessage("Memo must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Memo));
    }
}
