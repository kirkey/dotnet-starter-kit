namespace Accounting.Application.Invoices.LineItems.Add.v1;

/// <summary>
/// Handler for adding a line item to an invoice.
/// Updates the invoice total amount automatically.
/// </summary>
public sealed class AddInvoiceLineItemHandler(
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<AddInvoiceLineItemCommand, AddInvoiceLineItemResponse>
{
    public async Task<AddInvoiceLineItemResponse> Handle(AddInvoiceLineItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken);
        if (invoice is null)
        {
            throw new InvoiceNotFoundException(request.InvoiceId);
        }

        invoice.AddLineItem(
            description: request.Description,
            quantity: request.Quantity,
            unitPrice: request.UnitPrice,
            accountId: request.AccountId
        );

        await repository.UpdateAsync(invoice, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new AddInvoiceLineItemResponse(
            invoice.Id,
            invoice.TotalAmount
        );
    }
}

