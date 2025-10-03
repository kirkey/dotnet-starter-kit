using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts;

public sealed class SearchGoodsReceiptsSpec : Specification<GoodsReceipt>
{
    public SearchGoodsReceiptsSpec(SearchGoodsReceiptsCommand request)
    {
        Query
            .OrderByDescending(x => x.ReceivedDate)
            .ThenBy(x => x.ReceiptNumber)
            .Include(x => x.Items);

        if (!string.IsNullOrWhiteSpace(request.ReceiptNumber))
        {
            Query.Where(x => x.ReceiptNumber.Contains(request.ReceiptNumber));
        }

        if (request.PurchaseOrderId.HasValue)
        {
            Query.Where(x => x.PurchaseOrderId == request.PurchaseOrderId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            Query.Where(x => x.Status == request.Status);
        }

        if (request.ReceivedDateFrom.HasValue)
        {
            Query.Where(x => x.ReceivedDate >= request.ReceivedDateFrom.Value);
        }

        if (request.ReceivedDateTo.HasValue)
        {
            Query.Where(x => x.ReceivedDate <= request.ReceivedDateTo.Value);
        }
    }
}
