namespace Accounting.Application.WriteOffs.Create.v1;

/// <summary>
/// Validator for write-off creation command.
/// </summary>
public class WriteOffCreateCommandValidator : AbstractValidator<WriteOffCreateCommand>
{
    public WriteOffCreateCommandValidator()
    {
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty().WithMessage("Reference number is required")
            .MaximumLength(64).WithMessage("Reference number cannot exceed 50 characters");

        RuleFor(x => x.WriteOffDate)
            .NotEmpty().WithMessage("Write-off date is required");

        RuleFor(x => x.WriteOffType)
            .NotEmpty().WithMessage("Write-off type is required")
            .Must(type => new[] { "BadDebt", "CollectionAdjustment", "Discount", "Other" }.Contains(type))
            .WithMessage("Write-off type must be one of: BadDebt, CollectionAdjustment, Discount, Other");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Write-off amount must be positive");

        RuleFor(x => x.ReceivableAccountId)
            .NotEmpty().WithMessage("Receivable account ID is required");

        RuleFor(x => x.ExpenseAccountId)
            .NotEmpty().WithMessage("Expense account ID is required");

        RuleFor(x => x.CustomerName)
            .MaximumLength(256).WithMessage("Customer name cannot exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.CustomerName));

        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(64).WithMessage("Invoice number cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.InvoiceNumber));

        RuleFor(x => x.Reason)
            .MaximumLength(512).WithMessage("Reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Reason));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

