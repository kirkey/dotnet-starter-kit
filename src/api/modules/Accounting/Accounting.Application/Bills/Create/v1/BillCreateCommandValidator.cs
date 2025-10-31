namespace Accounting.Application.Bills.Create.v1;

/// <summary>
/// Validator for bill creation command.
/// </summary>
public class BillCreateCommandValidator : AbstractValidator<BillCreateCommand>
{
    public BillCreateCommandValidator()
    {
        RuleFor(x => x.BillNumber)
            .NotEmpty().WithMessage("Bill number is required")
            .MaximumLength(50).WithMessage("Bill number cannot exceed 50 characters");

        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("Vendor is required");

        RuleFor(x => x.VendorInvoiceNumber)
            .NotEmpty().WithMessage("Vendor invoice number is required")
            .MaximumLength(100).WithMessage("Vendor invoice number cannot exceed 100 characters");

        RuleFor(x => x.BillDate)
            .NotEmpty().WithMessage("Bill date is required");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required")
            .GreaterThanOrEqualTo(x => x.BillDate)
            .WithMessage("Due date must be on or after bill date");

        RuleFor(x => x.SubtotalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Subtotal amount cannot be negative");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Tax amount cannot be negative");

        RuleFor(x => x.ShippingAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Shipping amount cannot be negative");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount amount cannot be negative");

        RuleFor(x => x.PaymentTerms)
            .MaximumLength(100).WithMessage("Payment terms cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.PaymentTerms));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

