using Accounting.Application.Invoices.LineItems.Get.v1;

namespace Accounting.Application.Invoices.LineItems.GetList.v1;

/// <summary>
/// Handler for getting all line items for a specific invoice.
/// </summary>
public sealed class GetInvoiceLineItemsHandler(
    [FromKeyedServices("accounting:invoicelineitems")] IReadRepository<InvoiceLineItem> repository)
    : IRequestHandler<GetInvoiceLineItemsRequest, List<InvoiceLineItemResponse>>
{
    public async Task<List<InvoiceLineItemResponse>> Handle(GetInvoiceLineItemsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetInvoiceLineItemsSpec(request.InvoiceId);
        var lineItems = await repository.ListAsync(spec, cancellationToken);

        return lineItems.Select(li => new InvoiceLineItemResponse
        {
            Id = li.Id,
            InvoiceId = li.InvoiceId,
            Description = li.Description ?? string.Empty,
            Quantity = li.Quantity,
            UnitPrice = li.UnitPrice,
            TotalPrice = li.TotalPrice,
            AccountId = li.AccountId,
            CreatedOn = li.CreatedOn,
            LastModifiedOn = li.LastModifiedOn
        }).ToList();
    }
}

