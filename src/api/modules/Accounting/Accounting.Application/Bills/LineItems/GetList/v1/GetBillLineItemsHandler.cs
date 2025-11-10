using Accounting.Application.Bills.LineItems.Get.v1;

namespace Accounting.Application.Bills.LineItems.GetList.v1;

/// <summary>
/// Handler for getting all line items for a specific bill.
/// </summary>
public sealed class GetBillLineItemsHandler(
    [FromKeyedServices("accounting:bill-line-items")] IRepository<BillLineItem> repository)
    : IRequestHandler<GetBillLineItemsRequest, List<BillLineItemResponse>>
{
    public async Task<List<BillLineItemResponse>> Handle(GetBillLineItemsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetBillLineItemsByBillIdSpec(request.BillId);
        var lineItems = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        return lineItems.Adapt<List<BillLineItemResponse>>();
    }
}

