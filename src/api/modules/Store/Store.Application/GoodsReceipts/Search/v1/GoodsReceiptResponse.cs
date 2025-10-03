namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Search.v1;

public sealed record GoodsReceiptResponse
{
    public DefaultIdType Id { get; set; }
    public string ReceiptNumber { get; set; } = default!;
    public DefaultIdType? PurchaseOrderId { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string Status { get; set; } = default!;
    public int ItemCount { get; set; }
    public int TotalLines { get; set; }
    public int ReceivedLines { get; set; }
    public string? Notes { get; set; }
}
