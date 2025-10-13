using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts;

/// <summary>
/// Specification for searching goods receipts with various filters and pagination support.
/// </summary>
public sealed class SearchGoodsReceiptsSpec : EntitiesByPaginationFilterSpec<GoodsReceipt>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchGoodsReceiptsSpec"/> class.
    /// </summary>
    /// <param name="request">The search goods receipts command containing filter criteria and pagination parameters.</param>
    public SearchGoodsReceiptsSpec(SearchGoodsReceiptsCommand request)
        : base(request)
    {
        Query
            .Include(x => x.Items)
            .Where(x => x.ReceiptNumber.Contains(request.ReceiptNumber!), !string.IsNullOrWhiteSpace(request.ReceiptNumber))
            .Where(x => x.PurchaseOrderId == request.PurchaseOrderId!.Value, request.PurchaseOrderId.HasValue)
            .Where(x => x.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(x => x.ReceivedDate >= request.ReceivedDateFrom!.Value, request.ReceivedDateFrom.HasValue)
            .Where(x => x.ReceivedDate <= request.ReceivedDateTo!.Value, request.ReceivedDateTo.HasValue)
            .OrderByDescending(x => x.ReceivedDate, !request.HasOrderBy())
            .ThenBy(x => x.ReceiptNumber);
    }
}
