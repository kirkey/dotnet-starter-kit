namespace Accounting.Application.Invoices.LineItems.Update.v1;

/// <summary>
/// Handler for updating an invoice line item.
/// </summary>
public sealed class UpdateInvoiceLineItemHandler(
    [FromKeyedServices("accounting:invoicelineitems")] IRepository<InvoiceLineItem> repository)
    : IRequestHandler<UpdateInvoiceLineItemCommand, UpdateInvoiceLineItemResponse>
{
    public async Task<UpdateInvoiceLineItemResponse> Handle(UpdateInvoiceLineItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var lineItem = await repository.GetByIdAsync(request.LineItemId, cancellationToken);
        if (lineItem is null)
        {
            throw new InvoiceLineItemNotFoundException(request.LineItemId);
        }

        lineItem.Update(
            description: request.Description,
            quantity: request.Quantity,
            unitPrice: request.UnitPrice,
            accountId: request.AccountId
        );

        await repository.UpdateAsync(lineItem, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateInvoiceLineItemResponse(
            lineItem.Id,
            lineItem.TotalPrice
        );
    }
}

