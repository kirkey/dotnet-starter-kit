namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Get.v1;

/// <summary>
/// Response for get goods receipt operation.
/// </summary>
public sealed record GetGoodsReceiptResponse
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string ReceiptNumber { get; set; } = default!;
    public DefaultIdType? PurchaseOrderId { get; set; }
    public string? PurchaseOrderNumber { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string Status { get; set; } = default!;
    public string? Notes { get; set; }
    public IReadOnlyCollection<GoodsReceiptItemDto> Items { get; init; } = [];
}

/// <summary>
/// Data transfer object for goods receipt items.
/// </summary>
public sealed record GoodsReceiptItemDto
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string ItemName { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
}
