namespace Accounting.Application.Invoices.LineItems.Add.v1;

/// <summary>
/// Validator for add invoice line item command.
/// Ensures all required fields are provided and constraints are met.
/// </summary>
public sealed class AddInvoiceLineItemCommandValidator : AbstractValidator<AddInvoiceLineItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddInvoiceLineItemCommandValidator"/> class.
    /// </summary>
    public AddInvoiceLineItemCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty()
            .WithMessage("Invoice ID is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MinimumLength(3)
            .WithMessage("Description must be at least 3 characters.")
            .MaximumLength(512)
            .WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThan(1000000)
            .WithMessage("Quantity cannot exceed 999,999.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Unit price cannot be negative.")
            .LessThan(1000000)
            .WithMessage("Unit price cannot exceed 999,999.");
    }
}

