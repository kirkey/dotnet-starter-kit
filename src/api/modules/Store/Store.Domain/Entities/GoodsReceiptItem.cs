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
    /// Quantity received. Must be positive.
    /// Example: 100.
    /// </summary>
    public int Quantity { get; private set; }

    private GoodsReceiptItem() { }

    private GoodsReceiptItem(DefaultIdType id, DefaultIdType receiptId, DefaultIdType itemId, string name, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));

        Id = id;
        GoodsReceiptId = receiptId;
        ItemId = itemId;
        Name = name;
        Quantity = quantity;
    }

    /// <summary>
    /// Factory to create a goods receipt line item.
    /// </summary>
    /// <param name="receiptId">Parent receipt id.</param>
    /// <param name="itemId">Item received.</param>
    /// <param name="name">Item name. Example: "Bananas".</param>
    /// <param name="quantity">Received quantity. Example: 100.</param>
    public static GoodsReceiptItem Create(DefaultIdType receiptId, DefaultIdType itemId, string name, int quantity)
        => new(DefaultIdType.NewGuid(), receiptId, itemId, name, quantity);
}
