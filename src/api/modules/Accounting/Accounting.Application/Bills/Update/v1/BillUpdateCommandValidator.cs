namespace Accounting.Application.Bills.Update.v1;

/// <summary>
/// Validator for UpdateBillCommand with strict validation rules.
/// </summary>
public sealed class UpdateBillCommandValidator : AbstractValidator<UpdateBillCommand>
{
    public UpdateBillCommandValidator()
    {
        RuleFor(x => x.BillId)
            .NotEmpty()
            .WithMessage("Bill ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Bill ID must be a valid identifier.");

        RuleFor(x => x.BillNumber)
            .MaximumLength(64)
            .When(x => !string.IsNullOrWhiteSpace(x.BillNumber))
            .WithMessage("Bill number cannot exceed 50 characters.")
            .Must(BeValidBillNumber!)
            .When(x => !string.IsNullOrWhiteSpace(x.BillNumber))
            .WithMessage("Bill number contains invalid characters.");

        RuleFor(x => x.BillDate)
            .Must(date => date != default(DateTime))
            .When(x => x.BillDate.HasValue)
            .WithMessage("Bill date must be a valid date.")
            .LessThanOrEqualTo(DateTime.UtcNow.Date.AddDays(30))
            .When(x => x.BillDate.HasValue)
            .WithMessage("Bill date cannot be more than 30 days in the future.");

        RuleFor(x => x.DueDate)
            .Must(date => date != default(DateTime))
            .When(x => x.DueDate.HasValue)
            .WithMessage("Due date must be a valid date.");

        RuleFor(x => x.Description)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.PaymentTerms)
            .MaximumLength(128)
            .When(x => !string.IsNullOrWhiteSpace(x.PaymentTerms))
            .WithMessage("Payment terms cannot exceed 100 characters.");

        RuleFor(x => x.PurchaseOrderNumber)
            .MaximumLength(64)
            .When(x => !string.IsNullOrWhiteSpace(x.PurchaseOrderNumber))
            .WithMessage("Purchase order number cannot exceed 50 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes cannot exceed 2000 characters.");
    }

    private static bool BeValidBillNumber(string billNumber)
    {
        if (string.IsNullOrWhiteSpace(billNumber))
            return false;

        // Allow alphanumeric, dash, underscore, and space
        return billNumber.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == ' ');
    }
}

