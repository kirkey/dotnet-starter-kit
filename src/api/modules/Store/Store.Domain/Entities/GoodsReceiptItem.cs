namespace Store.Domain.Entities;

/// <summary>
/// Represents an item received into inventory during a goods receipt.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track individual items and quantities received from suppliers.
/// - Support partial deliveries against purchase orders.
/// - Provide audit trail for inventory increases.
/// </remarks>
/// <seealso cref="Store.Domain.Events.GoodsReceiptItemAdded"/>
/// <seealso cref="Store.Domain.Exceptions.GoodsReceipt.GoodsReceiptNotFoundException"/>
public sealed class GoodsReceiptItem : AuditableEntity
{
    /// <summary>
    /// Parent goods receipt identifier.
    /// Example: a <see cref="GoodsReceipt"/> Id.
    /// </summary>
    public DefaultIdType GoodsReceiptId { get; private set; }

    /// <summary>
    /// Item being received.
    /// Example: an existing <see cref="Item"/> Id.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Optional link to the purchase order item this receipt fulfills.
    /// Used for tracking partial receipts against PO line items.
    /// Example: Links this receipt to a specific PO line item for partial receiving.
    /// </summary>
    public DefaultIdType? PurchaseOrderItemId { get; private set; }

    /// <summary>
    /// Quantity received. Must be positive.
    /// Example: 100.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Cost per unit at time of receipt for inventory valuation.
    /// Example: 5.50 (currency based on system settings).
    /// </summary>
    public decimal UnitCost { get; private set; }

    /// <summary>
    /// Total cost calculated as Quantity × UnitCost.
    /// Example: If Quantity = 100 and UnitCost = 5.50, TotalCost = 550.00.
    /// </summary>
    public decimal TotalCost => Quantity * UnitCost;

    private GoodsReceiptItem() { }

    private GoodsReceiptItem(
        DefaultIdType id, 
        DefaultIdType receiptId, 
        DefaultIdType itemId, 
        string name, 
        int quantity, 
        decimal unitCost,
        DefaultIdType? purchaseOrderItemId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitCost < 0) throw new ArgumentException("UnitCost cannot be negative", nameof(unitCost));

        Id = id;
        GoodsReceiptId = receiptId;
        ItemId = itemId;
        Name = name;
        Quantity = quantity;
        UnitCost = unitCost;
        PurchaseOrderItemId = purchaseOrderItemId;
    }

    /// <summary>
    /// Factory to create a goods receipt line item.
    /// </summary>
    /// <param name="receiptId">Parent receipt id.</param>
    /// <param name="itemId">Item received.</param>
    /// <param name="name">Item name. Example: "Bananas".</param>
    /// <param name="quantity">Received quantity. Example: 100.</param>
    /// <param name="unitCost">Cost per unit. Example: 5.50.</param>
    /// <param name="purchaseOrderItemId">Optional PO item link for partial receiving tracking.</param>
    public static GoodsReceiptItem Create(
        DefaultIdType receiptId, 
        DefaultIdType itemId, 
        string name, 
        int quantity, 
        decimal unitCost,
        DefaultIdType? purchaseOrderItemId = null)
        => new(DefaultIdType.NewGuid(), receiptId, itemId, name, quantity, unitCost, purchaseOrderItemId);
}
