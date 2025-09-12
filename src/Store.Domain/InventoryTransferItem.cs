using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public class InventoryTransferItem : AuditableEntity, IAggregateRoot
{
    public DefaultIdType InventoryTransferId { get; private set; }
    public DefaultIdType GroceryItemId { get; private set; }
    public int RequestedQuantity { get; private set; }
    public int ShippedQuantity { get; private set; }
    public int ReceivedQuantity { get; private set; }
    public decimal UnitCost { get; private set; }
    public decimal TotalValue { get; private set; }
    public string? Notes { get; private set; }
    public string? BatchNumber { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    
    public virtual InventoryTransfer InventoryTransfer { get; private set; } = default!;
    public virtual GroceryItem GroceryItem { get; private set; } = default!;

    private InventoryTransferItem() { }

    private InventoryTransferItem(
        DefaultIdType id,
        DefaultIdType inventoryTransferId,
        DefaultIdType groceryItemId,
        int requestedQuantity,
        decimal unitCost,
        string? notes,
        string? batchNumber,
        DateTime? expiryDate)
    {
        Id = id;
        InventoryTransferId = inventoryTransferId;
        GroceryItemId = groceryItemId;
        RequestedQuantity = requestedQuantity;
        ShippedQuantity = 0;
        ReceivedQuantity = 0;
        UnitCost = unitCost;
        Notes = notes;
        BatchNumber = batchNumber;
        ExpiryDate = expiryDate;
        
        CalculateTotalValue();

        QueueDomainEvent(new InventoryTransferItemCreated { InventoryTransferItem = this });
    }

    public static InventoryTransferItem Create(
        DefaultIdType inventoryTransferId,
        DefaultIdType groceryItemId,
        int requestedQuantity,
        decimal unitCost,
        string? notes = null,
        string? batchNumber = null,
        DateTime? expiryDate = null)
    {
        return new InventoryTransferItem(
            DefaultIdType.NewGuid(),
            inventoryTransferId,
            groceryItemId,
            requestedQuantity,
            unitCost,
            notes,
            batchNumber,
            expiryDate);
    }

    public InventoryTransferItem Ship(int shippedQuantity)
    {
        if (shippedQuantity < 0 || shippedQuantity > RequestedQuantity)
        {
            throw new ArgumentException("Shipped quantity cannot be negative or exceed requested quantity.", nameof(shippedQuantity));
        }

        ShippedQuantity = shippedQuantity;
        QueueDomainEvent(new InventoryTransferItemShipped { InventoryTransferItem = this });
        return this;
    }

    public InventoryTransferItem Receive(int receivedQuantity)
    {
        if (receivedQuantity < 0 || receivedQuantity > ShippedQuantity)
        {
            throw new ArgumentException("Received quantity cannot be negative or exceed shipped quantity.", nameof(receivedQuantity));
        }

        ReceivedQuantity = receivedQuantity;
        QueueDomainEvent(new InventoryTransferItemReceived { InventoryTransferItem = this });
        return this;
    }

    private void CalculateTotalValue()
    {
        TotalValue = RequestedQuantity * UnitCost;
    }

    public bool IsFullyShipped() => ShippedQuantity >= RequestedQuantity;
    public bool IsFullyReceived() => ReceivedQuantity >= ShippedQuantity;
    public int GetPendingShipmentQuantity() => Math.Max(0, RequestedQuantity - ShippedQuantity);
    public int GetPendingReceiptQuantity() => Math.Max(0, ShippedQuantity - ReceivedQuantity);
    public bool HasDiscrepancy() => ReceivedQuantity != ShippedQuantity;
}
