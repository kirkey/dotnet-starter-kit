namespace Store.Domain.Entities;

/// <summary>
/// Line item inside an inventory transfer. Associates a grocery item with quantity and unit price.
/// </summary>
/// <remarks>
/// Use cases:
/// - Represent what is being shipped between warehouses.
/// - Compute line totals for transfer valuation.
/// </remarks>
public sealed class InventoryTransferItem : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Parent transfer id.
    /// </summary>
    public DefaultIdType InventoryTransferId { get; private set; }

    /// <summary>
    /// Item id being transferred.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Quantity being transferred. Must be > 0.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price used for valuation.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Computed line total: Quantity * UnitPrice.
    /// </summary>
    public decimal LineTotal { get; private set; }

    public InventoryTransfer InventoryTransfer { get; private set; } = default!;
    public Item Item { get; private set; } = default!;

    protected InventoryTransferItem() { }

    private InventoryTransferItem(DefaultIdType id, DefaultIdType inventoryTransferId, DefaultIdType itemId, int quantity, decimal unitPrice)
    {
        Id = id;
        InventoryTransferId = inventoryTransferId;
        ItemId = itemId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineTotal = quantity * unitPrice;

        QueueDomainEvent(new InventoryTransferItemCreated { InventoryTransferItem = this });
    }

    /// <summary>
    /// Creates a new <see cref="InventoryTransferItem"/> for the specified transfer.
    /// Validates required identifiers and business rules (positive quantity, non-negative unit price).
    /// </summary>
    /// <param name="inventoryTransferId">The parent inventory transfer id. Required.</param>
    /// <param name="itemId">The item id being transferred. Required.</param>
    /// <param name="quantity">Quantity to transfer. Must be &gt; 0.</param>
    /// <param name="unitPrice">Unit price for valuation. Must be &gt;= 0.</param>
    /// <returns>A new <see cref="InventoryTransferItem"/>.</returns>
    public static InventoryTransferItem Create(DefaultIdType inventoryTransferId, DefaultIdType itemId, int quantity, decimal unitPrice)
    {
        if (inventoryTransferId == default) throw new ArgumentException("InventoryTransferId is required", nameof(inventoryTransferId));
        if (itemId == default) throw new ArgumentException("ItemId is required", nameof(itemId));
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        return new InventoryTransferItem(DefaultIdType.NewGuid(), inventoryTransferId, itemId, quantity, unitPrice);
    }

    /// <summary>
    /// Updates quantity and unit price for this transfer item. Validates values and emits an update event when changed.
    /// This method does not itself check parent transfer state; callers (application layer) must ensure that the parent transfer can be modified.
    /// </summary>
    /// <param name="quantity">New quantity (&gt; 0).</param>
    /// <param name="unitPrice">New unit price (&gt;= 0).</param>
    /// <returns>The updated <see cref="InventoryTransferItem"/> instance.</returns>
    public InventoryTransferItem Update(int quantity, decimal unitPrice)
    {
        bool isUpdated = false;
        if (Quantity != quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
            Quantity = quantity;
            isUpdated = true;
        }
        if (UnitPrice != unitPrice)
        {
            if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
            UnitPrice = unitPrice;
            isUpdated = true;
        }
        if (isUpdated)
        {
            LineTotal = Quantity * UnitPrice;
            QueueDomainEvent(new InventoryTransferItemUpdated { InventoryTransferItem = this });
        }
        return this;
    }
}
