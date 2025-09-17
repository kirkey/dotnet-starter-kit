namespace Store.Domain;

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
    /// Grocery item id being transferred.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

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
    public GroceryItem GroceryItem { get; private set; } = default!;

    protected InventoryTransferItem() { }

    private InventoryTransferItem(DefaultIdType id, DefaultIdType inventoryTransferId, DefaultIdType groceryItemId, int quantity, decimal unitPrice)
    {
        Id = id;
        InventoryTransferId = inventoryTransferId;
        GroceryItemId = groceryItemId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineTotal = quantity * unitPrice;

        QueueDomainEvent(new InventoryTransferItemCreated { InventoryTransferItem = this });
    }

    public static InventoryTransferItem Create(DefaultIdType inventoryTransferId, DefaultIdType groceryItemId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));
        return new InventoryTransferItem(DefaultIdType.NewGuid(), inventoryTransferId, groceryItemId, quantity, unitPrice);
    }

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
