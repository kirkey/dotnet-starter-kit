namespace Accounting.Application.Invoices.LineItems.Update.v1;

/// <summary>
/// Validator for update invoice line item command.
/// Ensures all provided fields meet the required constraints.
/// </summary>
public sealed class UpdateInvoiceLineItemCommandValidator : AbstractValidator<UpdateInvoiceLineItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInvoiceLineItemCommandValidator"/> class.
    /// </summary>
    public UpdateInvoiceLineItemCommandValidator()
    {
        RuleFor(x => x.LineItemId)
            .NotEmpty()
            .WithMessage("Line item ID is required.");

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description!)
                .MinimumLength(3)
                .WithMessage("Description must be at least 3 characters.")
                .MaximumLength(512)
                .WithMessage("Description cannot exceed 500 characters.");
        });

        When(x => x.Quantity.HasValue, () =>
        {
            RuleFor(x => x.Quantity!.Value)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.")
                .LessThan(1000000)
                .WithMessage("Quantity cannot exceed 999,999.");
        });

        When(x => x.UnitPrice.HasValue, () =>
        {
            RuleFor(x => x.UnitPrice!.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Unit price cannot be negative.")
                .LessThan(1000000)
                .WithMessage("Unit price cannot exceed 999,999.");
        });
    }
}

