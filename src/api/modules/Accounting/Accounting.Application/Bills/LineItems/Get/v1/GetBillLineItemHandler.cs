namespace Accounting.Application.Bills.LineItems.Get.v1;

/// <summary>
/// Handler for getting a single bill line item by ID.
/// </summary>
public sealed class GetBillLineItemHandler(
    [FromKeyedServices("accounting:billlineitems")] IRepository<BillLineItem> repository)
    : IRequestHandler<GetBillLineItemRequest, BillLineItemResponse>
{
    public async Task<BillLineItemResponse> Handle(GetBillLineItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var lineItem = await repository.GetByIdAsync(request.LineItemId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillLineItemNotFoundException(request.LineItemId);

        return lineItem.Adapt<BillLineItemResponse>();
    }
}
