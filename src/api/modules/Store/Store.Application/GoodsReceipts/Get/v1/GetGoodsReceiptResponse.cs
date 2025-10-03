namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Get.v1;

public sealed record GetGoodsReceiptResponse
{
    public DefaultIdType Id { get; set; }
    public string ReceiptNumber { get; set; } = default!;
    public DefaultIdType? PurchaseOrderId { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string Status { get; set; } = default!;
    public IReadOnlyCollection<GoodsReceiptItemDto> Items { get; init; } = [];
}

public sealed record GoodsReceiptItemDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
}
