namespace Accounting.Application.Invoices.LineItems.Delete.v1;

/// <summary>
/// Validator for delete invoice line item command.
/// Ensures the line item ID is provided.
/// </summary>
public sealed class DeleteInvoiceLineItemCommandValidator : AbstractValidator<DeleteInvoiceLineItemCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteInvoiceLineItemCommandValidator"/> class.
    /// </summary>
    public DeleteInvoiceLineItemCommandValidator()
    {
        RuleFor(x => x.LineItemId)
            .NotEmpty()
            .WithMessage("Line item ID is required.");
    }
}

