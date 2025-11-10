namespace Accounting.Application.Invoices.LineItems.Delete.v1;

/// <summary>
/// Handler for deleting an invoice line item.
/// </summary>
public sealed class DeleteInvoiceLineItemHandler(
    [FromKeyedServices("accounting:invoice-line-items")] IRepository<InvoiceLineItem> repository)
    : IRequestHandler<DeleteInvoiceLineItemCommand, DeleteInvoiceLineItemResponse>
{
    public async Task<DeleteInvoiceLineItemResponse> Handle(DeleteInvoiceLineItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var lineItem = await repository.GetByIdAsync(request.LineItemId, cancellationToken);
        if (lineItem is null)
        {
            throw new InvoiceLineItemNotFoundException(request.LineItemId);
        }

        await repository.DeleteAsync(lineItem, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new DeleteInvoiceLineItemResponse(request.LineItemId, true);
    }
}

