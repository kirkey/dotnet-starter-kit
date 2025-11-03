namespace Store.Domain.Entities;

/// <summary>
/// Represents an inbound goods receipt (typically against a Purchase Order) into the warehouse.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record delivery of goods from suppliers.
/// - Match received items against purchase orders.
/// - Update inventory levels when goods arrive.
/// - Track receipt status and completion.
/// </remarks>
/// <seealso cref="Store.Domain.Events.GoodsReceiptCreated"/>
/// <seealso cref="Store.Domain.Events.GoodsReceiptItemAdded"/>
/// <seealso cref="Store.Domain.Events.GoodsReceiptCompleted"/>
/// <seealso cref="Store.Domain.Exceptions.GoodsReceipt.GoodsReceiptNotFoundException"/>
public sealed class GoodsReceipt : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique receipt number for tracking deliveries.
    /// Example: "GR-2025-09-001". Max length: 100.
    /// </summary>
    public string ReceiptNumber { get; private set; } = default!;

    /// <summary>
    /// Optional reference to the purchase order being fulfilled.
    /// Example: an existing <see cref="PurchaseOrder"/> Id or null for ad-hoc receipts.
    /// </summary>
    public DefaultIdType? PurchaseOrderId { get; private set; }

    /// <summary>
    /// Warehouse where goods are being received.
    /// Example: Main Warehouse ID. Required for inventory tracking.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Optional specific location within the warehouse where goods are received.
    /// Example: Receiving Dock A, Aisle 5, etc.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Date when goods were received.
    /// Example: 2025-09-18T09:30:00Z. Defaults to current UTC if unspecified.
    /// </summary>
    public DateTime ReceivedDate { get; private set; }

    /// <summary>
    /// Receipt processing status.
    /// Allowed values: Open, Received, Cancelled. Default: Open.
    /// </summary>
    public string Status { get; private set; } = "Open"; // Open, Received, Cancelled

    private readonly List<GoodsReceiptItem> _items = new();
    /// <summary>
    /// Collection of goods receipt items, each representing a quantity received for a specific item.
    /// Read-only to enforce proper aggregate management.
    /// </summary>
    public IReadOnlyCollection<GoodsReceiptItem> Items => _items.AsReadOnly();

    private GoodsReceipt() { }

    private GoodsReceipt(
        DefaultIdType id, 
        string receiptNumber, 
        DateTime receivedDate, 
        DefaultIdType warehouseId, 
        DefaultIdType? warehouseLocationId,
        DefaultIdType? purchaseOrderId, 
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(receiptNumber)) throw new ArgumentException("ReceiptNumber is required", nameof(receiptNumber));
        if (receiptNumber.Length > 100) throw new ArgumentException("ReceiptNumber must not exceed 100 characters", nameof(receiptNumber));
        if (notes?.Length > 2048) throw new ArgumentException("Notes must not exceed 2048 characters", nameof(notes));
        if (warehouseId == default) throw new ArgumentException("WarehouseId is required", nameof(warehouseId));

        Id = id;
        ReceiptNumber = receiptNumber;
        ReceivedDate = receivedDate == default ? DateTime.UtcNow : receivedDate;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        PurchaseOrderId = purchaseOrderId;
        Notes = notes; // Inherited from AuditableEntity base class
        Status = "Open";
        QueueDomainEvent(new GoodsReceiptCreated { GoodsReceipt = this });
    }

    /// <summary>
    /// Factory to create a new goods receipt.
    /// </summary>
    /// <param name="receiptNumber">Unique receipt number. Example: "GR-2025-09-001".</param>
    /// <param name="receivedDate">Receipt date. Defaults to now if unspecified.</param>
    /// <param name="warehouseId">Warehouse where goods are received. Required.</param>
    /// <param name="warehouseLocationId">Optional specific location within warehouse.</param>
    /// <param name="purchaseOrderId">Optional PO reference.</param>
    /// <param name="notes">Optional notes. Max length: 2048 characters.</param>
    public static GoodsReceipt Create(
        string receiptNumber, 
        DateTime receivedDate, 
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId = null,
        DefaultIdType? purchaseOrderId = null, 
        string? notes = null)
        => new(DefaultIdType.NewGuid(), receiptNumber, receivedDate, warehouseId, warehouseLocationId, purchaseOrderId, notes);

    /// <summary>
    /// Adds an item to the goods receipt.
    /// </summary>
    /// <param name="itemId">Item received.</param>
    /// <param name="name">Item name snapshot. Example: "Bananas".</param>
    /// <param name="quantity">Received quantity. Example: 100.</param>
    /// <param name="unitCost">Cost per unit for inventory valuation. Example: 5.50.</param>
    /// <param name="purchaseOrderItemId">Optional PO item ID for tracking partial receipts.</param>
    public GoodsReceipt AddItem(DefaultIdType itemId, string name, int quantity, decimal unitCost, DefaultIdType? purchaseOrderItemId = null)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (unitCost < 0) throw new ArgumentException("UnitCost cannot be negative", nameof(unitCost));
        var item = GoodsReceiptItem.Create(Id, itemId, name, quantity, unitCost, purchaseOrderItemId);
        _items.Add(item);
        QueueDomainEvent(new GoodsReceiptItemAdded { GoodsReceipt = this, Item = item });
        return this;
    }

    /// <summary>
    /// Marks the receipt as received/completed.
    /// </summary>
    public GoodsReceipt MarkReceived()
    {
        if (string.Equals(Status, "Received", StringComparison.OrdinalIgnoreCase)) return this;
        Status = "Received";
        QueueDomainEvent(new GoodsReceiptCompleted { GoodsReceipt = this });
        return this;
    }
}
