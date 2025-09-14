using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class InventoryTransfer : AuditableEntity, IAggregateRoot
{
    public string? Reason { get; private set; }
    public string TransferNumber { get; private set; } = default!;
    public DefaultIdType FromWarehouseId { get; private set; }
    public DefaultIdType ToWarehouseId { get; private set; }
    public DefaultIdType? FromLocationId { get; private set; }
    public DefaultIdType? ToLocationId { get; private set; }
    public DateTime TransferDate { get; private set; }
    public DateTime? ExpectedArrivalDate { get; private set; }
    public DateTime? ActualArrivalDate { get; private set; }
    public string Status { get; private set; } = default!; // Pending, Approved, InTransit, Completed, Cancelled
    public string TransferType { get; private set; } = default!; // Standard, Express, etc.
    public string Priority { get; private set; } = default!; // Low, Normal, High, Critical
    public decimal TotalValue { get; private set; }
    public string? TransportMethod { get; private set; }
    public string? TrackingNumber { get; private set; }
    public string? RequestedBy { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovalDate { get; private set; }

    public ICollection<InventoryTransferItem> Items { get; private set; } = new List<InventoryTransferItem>();

    public Warehouse FromWarehouse { get; private set; } = default!;
    public Warehouse ToWarehouse { get; private set; } = default!;
    public WarehouseLocation? FromLocation { get; private set; }
    public WarehouseLocation? ToLocation { get; private set; }

    private InventoryTransfer() { }

    private InventoryTransfer(
        DefaultIdType id,
        string transferNumber,
        DefaultIdType fromWarehouseId,
        DefaultIdType toWarehouseId,
        DefaultIdType? fromLocationId,
        DefaultIdType? toLocationId,
        DateTime transferDate,
        DateTime? expectedArrivalDate,
        string transferType,
        string priority,
        string? transportMethod,
        string? notes,
        string? requestedBy)
    {
        Id = id;
        TransferNumber = transferNumber;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        FromLocationId = fromLocationId;
        ToLocationId = toLocationId;
        TransferDate = transferDate;
        ExpectedArrivalDate = expectedArrivalDate;
        TransferType = transferType;
        Priority = priority;
        TransportMethod = transportMethod;
        Notes = notes; // Notes property comes from AuditableEntity
        RequestedBy = requestedBy;
        Status = "Pending";
        TotalValue = 0m;

        QueueDomainEvent(new InventoryTransferCreated { InventoryTransfer = this });
    }

    public static InventoryTransfer Create(
        string? transferNumber,
        object? unused,
        string transferNumberOverride,
        DefaultIdType fromWarehouseId,
        DefaultIdType toWarehouseId,
        DefaultIdType? fromLocationId,
        DefaultIdType? toLocationId,
        DateTime transferDate,
        DateTime? expectedArrivalDate,
        string transferType,
        string priority,
        string? transportMethod,
        string? notes,
        string? requestedBy)
    {
        // some callers pass nulls differently; use transferNumberOverride if provided else generate
        var tn = string.IsNullOrWhiteSpace(transferNumberOverride) ? transferNumber ?? $"TRF-{DefaultIdType.NewGuid()}" : transferNumberOverride;
        return new InventoryTransfer(
            DefaultIdType.NewGuid(),
            tn,
            fromWarehouseId,
            toWarehouseId,
            fromLocationId,
            toLocationId,
            transferDate,
            expectedArrivalDate,
            transferType,
            priority,
            transportMethod,
            notes,
            requestedBy);
    }

    public InventoryTransfer AddItem(DefaultIdType groceryItemId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        var item = InventoryTransferItem.Create(Id, groceryItemId, quantity, unitPrice);
        Items.Add(item);
        RecalculateTotal();
        return this;
    }

    public InventoryTransfer RemoveItem(DefaultIdType itemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotal();
        }
        return this;
    }

    private void RecalculateTotal()
    {
        TotalValue = Items.Sum(i => i.Quantity * i.UnitPrice);
    }

    public InventoryTransfer Approve(string approvedBy)
    {
        if (Status != "Pending") return this;
        Status = "Approved";
        ApprovedBy = approvedBy;
        ApprovalDate = DateTime.UtcNow;
        QueueDomainEvent(new InventoryTransferApproved { InventoryTransfer = this });
        return this;
    }

    public InventoryTransfer MarkInTransit()
    {
        if (Status == "Approved")
        {
            Status = "InTransit";
            QueueDomainEvent(new InventoryTransferInTransit { InventoryTransfer = this });
        }
        return this;
    }

    public InventoryTransfer Complete(DateTime actualArrival)
    {
        if (Status == "InTransit")
        {
            Status = "Completed";
            ActualArrivalDate = actualArrival;
            QueueDomainEvent(new InventoryTransferCompleted { InventoryTransfer = this });
        }
        return this;
    }

    public InventoryTransfer Cancel()
    {
        if (Status != "Completed")
        {
            Status = "Cancelled";
            QueueDomainEvent(new InventoryTransferCancelled { InventoryTransfer = this });
        }
        return this;
    }

    // New helper to set tracking number (used when shipment/tracking info is available)
    public InventoryTransfer SetTrackingNumber(string? trackingNumber)
    {
        if (!string.Equals(TrackingNumber, trackingNumber, StringComparison.OrdinalIgnoreCase))
        {
            TrackingNumber = trackingNumber;
            QueueDomainEvent(new InventoryTransferUpdated { InventoryTransfer = this });
        }
        return this;
    }

    // Update method used by application layer to apply editable fields
    public InventoryTransfer Update(
        string name,
        string? description,
        string transferNumber,
        DefaultIdType fromWarehouseId,
        DefaultIdType toWarehouseId,
        DateTime transferDate,
        string status,
        string? notes,
        string? reason)
    {
        var changed = false;

        if (Name != name)
        {
            Name = name; // Name inherited from AuditableEntity
            changed = true;
        }

        if (Description != description)
        {
            Description = description; // inherited
            changed = true;
        }

        if (TransferNumber != transferNumber)
        {
            TransferNumber = transferNumber;
            changed = true;
        }

        if (FromWarehouseId != fromWarehouseId)
        {
            FromWarehouseId = fromWarehouseId;
            changed = true;
        }

        if (ToWarehouseId != toWarehouseId)
        {
            ToWarehouseId = toWarehouseId;
            changed = true;
        }

        if (TransferDate != transferDate)
        {
            TransferDate = transferDate;
            changed = true;
        }

        if (Status != status)
        {
            Status = status;
            changed = true;
        }

        if (Notes != notes)
        {
            Notes = notes; // inherited
            changed = true;
        }

        if (Reason != reason)
        {
            Reason = reason;
            changed = true;
        }

        if (changed)
        {
            QueueDomainEvent(new InventoryTransferUpdated { InventoryTransfer = this });
        }

        return this;
    }
}
