namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.Queries;

/// <summary>
/// Query to get available items from a purchase order for receiving.
/// Returns items that haven't been fully received yet.
/// </summary>
public sealed record GetPurchaseOrderItemsForReceivingQuery : IRequest<GetPurchaseOrderItemsForReceivingResponse>
{
    public DefaultIdType PurchaseOrderId { get; set; }
}

/// <summary>
/// Response containing items available for receiving with their pending quantities.
/// </summary>
public sealed record GetPurchaseOrderItemsForReceivingResponse
{
    public DefaultIdType PurchaseOrderId { get; set; }
    public string OrderNumber { get; set; } = default!;
    public string Status { get; set; } = default!;
    public List<PurchaseOrderItemForReceiving> Items { get; set; } = new();
}

/// <summary>
/// Represents a PO item with receiving information.
/// </summary>
public sealed record PurchaseOrderItemForReceiving
{
    public DefaultIdType PurchaseOrderItemId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string ItemName { get; set; } = default!;
    public string ItemSku { get; set; } = default!;
    public int OrderedQuantity { get; set; }
    public int ReceivedQuantity { get; set; }
    public int RemainingQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsFullyReceived { get; set; }
}

