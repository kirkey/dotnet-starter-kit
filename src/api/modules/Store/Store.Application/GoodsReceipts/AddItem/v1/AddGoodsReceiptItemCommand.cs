namespace FSH.Starter.WebApi.Store.Application.GoodsReceipts.AddItem.v1;

public sealed record AddGoodsReceiptItemCommand : IRequest<AddGoodsReceiptItemResponse>
{
    public DefaultIdType GoodsReceiptId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    
    /// <summary>
    /// Optional link to PurchaseOrderItem for tracking partial receipts.
    /// When provided, the system will update the PO item's ReceivedQuantity.
    /// </summary>
    public DefaultIdType? PurchaseOrderItemId { get; set; }
}
