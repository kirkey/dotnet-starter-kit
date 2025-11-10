namespace Accounting.Application.Invoices.LineItems.Get.v1;

/// <summary>
/// Handler for getting a specific invoice line item.
/// </summary>
public sealed class GetInvoiceLineItemHandler(
    [FromKeyedServices("accounting:invoice-line-items")] IReadRepository<InvoiceLineItem> repository)
    : IRequestHandler<GetInvoiceLineItemRequest, InvoiceLineItemResponse>
{
    public async Task<InvoiceLineItemResponse> Handle(GetInvoiceLineItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var lineItem = await repository.GetByIdAsync(request.LineItemId, cancellationToken).ConfigureAwait(false);
        if (lineItem is null)
        {
            throw new InvoiceLineItemNotFoundException(request.LineItemId);
        }

        return new InvoiceLineItemResponse
        {
            Id = lineItem.Id,
            InvoiceId = lineItem.InvoiceId,
            Description = lineItem.Description ?? string.Empty,
            Quantity = lineItem.Quantity,
            UnitPrice = lineItem.UnitPrice,
            TotalPrice = lineItem.TotalPrice,
            AccountId = lineItem.AccountId,
            CreatedOn = lineItem.CreatedOn,
            LastModifiedOn = lineItem.LastModifiedOn
        };
    }
}

