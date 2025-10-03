namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Search.v1;

public class SearchGoodsReceiptsCommand : PaginationFilter, IRequest<PagedList<GoodsReceiptResponse>>
{
    public string? ReceiptNumber { get; set; }
    public DefaultIdType? PurchaseOrderId { get; set; }
    public string? Status { get; set; }
    public DateTime? ReceivedDateFrom { get; set; }
    public DateTime? ReceivedDateTo { get; set; }
}
