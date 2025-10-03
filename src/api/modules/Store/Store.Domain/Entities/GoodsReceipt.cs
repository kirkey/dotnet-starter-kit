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
    /// Date when goods were received.
    /// Example: 2025-09-18T09:30:00Z. Defaults to current UTC if unspecified.
    /// </summary>
    public DateTime ReceivedDate { get; private set; }

    /// <summary>
    /// Receipt processing status.
    /// Allowed values: Open, Received, Cancelled. Default: Open.
    /// </summary>
    public string Status { get; private set; } = "Open"; // Open, Received, Cancelled

    /// <summary>
    /// Items received in this delivery.
    /// Example count: 0 at creation.
    /// </summary>
    public ICollection<GoodsReceiptItem> Items { get; private set; } = new List<GoodsReceiptItem>();

    private GoodsReceipt() { }

    private GoodsReceipt(DefaultIdType id, string receiptNumber, DateTime receivedDate, DefaultIdType? purchaseOrderId)
    {
        if (string.IsNullOrWhiteSpace(receiptNumber)) throw new ArgumentException("ReceiptNumber is required", nameof(receiptNumber));
        if (receiptNumber.Length > 100) throw new ArgumentException("ReceiptNumber must not exceed 100 characters", nameof(receiptNumber));

        Id = id;
        ReceiptNumber = receiptNumber;
        ReceivedDate = receivedDate == default ? DateTime.UtcNow : receivedDate;
        PurchaseOrderId = purchaseOrderId;
        Status = "Open";
        QueueDomainEvent(new GoodsReceiptCreated { GoodsReceipt = this });
    }

    /// <summary>
    /// Factory to create a new goods receipt.
    /// </summary>
    /// <param name="receiptNumber">Unique receipt number. Example: "GR-2025-09-001".</param>
    /// <param name="receivedDate">Receipt date. Defaults to now if unspecified.</param>
    /// <param name="purchaseOrderId">Optional PO reference.</param>
    public static GoodsReceipt Create(string receiptNumber, DateTime receivedDate, DefaultIdType? purchaseOrderId = null)
        => new(DefaultIdType.NewGuid(), receiptNumber, receivedDate, purchaseOrderId);

    /// <summary>
    /// Adds an item to the receipt and updates inventory.
    /// </summary>
    /// <param name="groceryItemId">Item received.</param>
    /// <param name="name">Item name snapshot. Example: "Bananas".</param>
    /// <param name="quantity">Received quantity. Example: 100.</param>
    public GoodsReceipt AddItem(DefaultIdType groceryItemId, string name, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        var item = GoodsReceiptItem.Create(Id, groceryItemId, name, quantity);
        Items.Add(item);
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
