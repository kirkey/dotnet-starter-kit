namespace Accounting.Application.PurchaseOrders.Create.v1;

/// <summary>
/// Validator for purchase order creation command.
/// </summary>
public class PurchaseOrderCreateCommandValidator : AbstractValidator<PurchaseOrderCreateCommand>
{
    public PurchaseOrderCreateCommandValidator()
    {
        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order number is required")
            .MaximumLength(50).WithMessage("Order number cannot exceed 50 characters");

        RuleFor(x => x.OrderDate)
            .NotEmpty().WithMessage("Order date is required");

        RuleFor(x => x.VendorId)
            .NotEmpty().WithMessage("Vendor ID is required");

        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("Vendor name is required")
            .MaximumLength(256).WithMessage("Vendor name cannot exceed 256 characters");

        RuleFor(x => x.ExpectedDeliveryDate)
            .GreaterThanOrEqualTo(x => x.OrderDate).WithMessage("Expected delivery date cannot be before order date")
            .When(x => x.ExpectedDeliveryDate.HasValue);

        RuleFor(x => x.ShipToAddress)
            .MaximumLength(500).WithMessage("Ship to address cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ShipToAddress));

        RuleFor(x => x.PaymentTerms)
            .MaximumLength(100).WithMessage("Payment terms cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.PaymentTerms));

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(100).WithMessage("Reference number cannot exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.RequesterName)
            .MaximumLength(256).WithMessage("Requester name cannot exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.RequesterName));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

