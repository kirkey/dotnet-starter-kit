using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class InventoryTransferItem : AuditableEntity, IAggregateRoot
{
    public DefaultIdType InventoryTransferId { get; private set; }
    public DefaultIdType GroceryItemId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
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
