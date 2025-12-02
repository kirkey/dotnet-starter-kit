namespace Accounting.Application.InterCompanyTransactions.Create.v1;

/// <summary>
/// Validator for inter-company transaction creation command.
/// </summary>
public class InterCompanyTransactionCreateCommandValidator : AbstractValidator<InterCompanyTransactionCreateCommand>
{
    public InterCompanyTransactionCreateCommandValidator()
    {
        RuleFor(x => x.TransactionNumber)
            .NotEmpty().WithMessage("Transaction number is required")
            .MaximumLength(64).WithMessage("Transaction number cannot exceed 50 characters");

        RuleFor(x => x.FromEntityId)
            .NotEmpty().WithMessage("From entity ID is required");

        RuleFor(x => x.FromEntityName)
            .NotEmpty().WithMessage("From entity name is required")
            .MaximumLength(256).WithMessage("From entity name cannot exceed 256 characters");

        RuleFor(x => x.ToEntityId)
            .NotEmpty().WithMessage("To entity ID is required")
            .NotEqual(x => x.FromEntityId).WithMessage("From entity and to entity must be different");

        RuleFor(x => x.ToEntityName)
            .NotEmpty().WithMessage("To entity name is required")
            .MaximumLength(256).WithMessage("To entity name cannot exceed 256 characters");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive");

        RuleFor(x => x.TransactionType)
            .NotEmpty().WithMessage("Transaction type is required")
            .MaximumLength(64).WithMessage("Transaction type cannot exceed 50 characters")
            .Must(type => new[] { "Billing", "Loan", "Advance", "Allocation", "Dividend", "CapitalContribution", "Settlement", "Other" }.Contains(type))
            .WithMessage("Transaction type must be one of: Billing, Loan, Advance, Allocation, Dividend, CapitalContribution, Settlement, Other");

        RuleFor(x => x.FromAccountId)
            .NotEmpty().WithMessage("From account ID is required");

        RuleFor(x => x.ToAccountId)
            .NotEmpty().WithMessage("To account ID is required");

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(x => x.TransactionDate).WithMessage("Due date cannot be before transaction date")
            .When(x => x.DueDate.HasValue);

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(128).WithMessage("Reference number cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

