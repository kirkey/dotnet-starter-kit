using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public class InventoryTransfer : AuditableEntity, IAggregateRoot
{
    public string TransferNumber { get; private set; } = default!;
    // Name and Description are inherited from AuditableEntity
    public DefaultIdType FromWarehouseId { get; private set; }
    public DefaultIdType ToWarehouseId { get; private set; }
    public DefaultIdType? FromLocationId { get; private set; }
    public DefaultIdType? ToLocationId { get; private set; }
    public DateTime TransferDate { get; private set; }
    public DateTime? ExpectedArrivalDate { get; private set; }
    public DateTime? ActualArrivalDate { get; private set; }
    public string Status { get; private set; } = default!; // Pending, InTransit, Completed, Cancelled
    public string TransferType { get; private set; } = default!; // Replenishment, Rebalancing, Emergency, Consolidation
    public string Priority { get; private set; } = default!; // Low, Normal, High, Critical
    public decimal TotalValue { get; private set; }
    public string? TransportMethod { get; private set; }
    public string? TrackingNumber { get; private set; }
    // Notes is inherited from AuditableEntity
    public string? RequestedBy { get; private set; }
    public string? Reason { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovalDate { get; private set; }
    
    public virtual Warehouse FromWarehouse { get; private set; } = default!;
    public virtual Warehouse ToWarehouse { get; private set; } = default!;
    public virtual WarehouseLocation? FromLocation { get; private set; }
    public virtual WarehouseLocation? ToLocation { get; private set; }
    public virtual ICollection<InventoryTransferItem> Items { get; private set; } = new List<InventoryTransferItem>();

    private InventoryTransfer() { }

    private InventoryTransfer(
        DefaultIdType id,
        string? name,
        string? description,
        string transferNumber,
        DefaultIdType fromWarehouseId,
        DefaultIdType toWarehouseId,
        DefaultIdType? fromLocationId,
        DefaultIdType? toLocationId,
        DateTime transferDate,
        DateTime? expectedArrivalDate,
        string status,
        string transferType,
        string priority,
        string? transportMethod,
        string? notes,
        string? requestedBy)
    {
        Id = id;
        Name = name;
        Description = description;
        TransferNumber = transferNumber;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        FromLocationId = fromLocationId;
        ToLocationId = toLocationId;
        TransferDate = transferDate;
        ExpectedArrivalDate = expectedArrivalDate;
        Status = status;
        TransferType = transferType;
        Priority = priority;
        TotalValue = 0;
        TransportMethod = transportMethod;
        Notes = notes;
        RequestedBy = requestedBy;

        QueueDomainEvent(new InventoryTransferCreated { InventoryTransfer = this });
    }

    public static InventoryTransfer Create(
        string? name,
        string? description,
        string transferNumber,
        DefaultIdType fromWarehouseId,
        DefaultIdType toWarehouseId,
        DefaultIdType? fromLocationId,
        DefaultIdType? toLocationId,
        DateTime transferDate,
        DateTime? expectedArrivalDate,
        string transferType,
        string priority = "Normal",
        string? transportMethod = null,
        string? notes = null,
        string? requestedBy = null)
    {
        return new InventoryTransfer(
            DefaultIdType.NewGuid(),
            name,
            description,
            transferNumber,
            fromWarehouseId,
            toWarehouseId,
            fromLocationId,
            toLocationId,
            transferDate,
            expectedArrivalDate,
            "Pending",
            transferType,
            priority,
            transportMethod,
            notes,
            requestedBy);
    }

    public InventoryTransfer AddItem(DefaultIdType groceryItemId, int quantity, decimal unitCost)
    {
        var item = InventoryTransferItem.Create(Id, groceryItemId, quantity, unitCost);
        Items.Add(item);
        RecalculateTotalValue();
        QueueDomainEvent(new InventoryTransferItemAdded { InventoryTransfer = this, GroceryItemId = groceryItemId });
        return this;
    }

    public InventoryTransfer Approve(string approvedBy)
    {
        if (Status == "Pending")
        {
            ApprovedBy = approvedBy;
            ApprovalDate = DateTime.UtcNow;
            Status = "Approved";
            QueueDomainEvent(new InventoryTransferApproved { InventoryTransfer = this });
        }
        return this;
    }

    public InventoryTransfer StartTransit(string? trackingNumber = null)
    {
        if (Status == "Approved")
        {
            Status = "InTransit";
            TrackingNumber = trackingNumber;
            QueueDomainEvent(new InventoryTransferStarted { InventoryTransfer = this });
        }
        return this;
    }

    public InventoryTransfer Complete(DateTime actualArrivalDate)
    {
        if (Status == "InTransit")
        {
            Status = "Completed";
            ActualArrivalDate = actualArrivalDate;
            QueueDomainEvent(new InventoryTransferCompleted { InventoryTransfer = this });
        }
        return this;
    }

    public InventoryTransfer Cancel(string reason)
    {
        if (Status != "Completed")
        {
            Status = "Cancelled";
            Notes = string.IsNullOrEmpty(Notes) ? reason : $"{Notes}; Cancelled: {reason}";
            QueueDomainEvent(new InventoryTransferCancelled { InventoryTransfer = this, Reason = reason });
        }
        return this;
    }

    public InventoryTransfer Update(
        string? name,
        string? description,
        string transferNumber,
        DefaultIdType fromWarehouseId,
        DefaultIdType toWarehouseId,
        DateTime transferDate,
        string status,
        string? notes,
        string reason)
    {
        bool isUpdated = false;

        if (!string.Equals(TransferNumber, transferNumber, StringComparison.OrdinalIgnoreCase))
        {
            TransferNumber = transferNumber;
            isUpdated = true;
        }
        if (!string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }
        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }
        if (FromWarehouseId != fromWarehouseId)
        {
            FromWarehouseId = fromWarehouseId;
            isUpdated = true;
        }

        if (ToWarehouseId != toWarehouseId)
        {
            ToWarehouseId = toWarehouseId;
            isUpdated = true;
        }

        if (TransferDate != transferDate)
        {
            TransferDate = transferDate;
            isUpdated = true;
        }

        if (Status != status)
        {
            Status = status;
            isUpdated = true;
        }

        if (!string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes;
            isUpdated = true;
        }

        if (!string.Equals(Reason, reason, StringComparison.OrdinalIgnoreCase))
        {
            Reason = reason;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InventoryTransferUpdated { InventoryTransfer = this });
        }

        return this;
    }

    private void RecalculateTotalValue()
    {
        TotalValue = Items.Sum(i => i.TotalValue);
    }

    public bool IsOverdue() => 
        ExpectedArrivalDate.HasValue && 
        ExpectedArrivalDate.Value < DateTime.UtcNow && 
        Status == "InTransit";

    public bool IsHighPriority() => Priority == "High" || Priority == "Critical";
    public bool IsCompleted() => Status == "Completed";
    public int GetDaysInTransit() => ActualArrivalDate.HasValue 
        ? (ActualArrivalDate.Value - TransferDate).Days 
        : (DateTime.UtcNow - TransferDate).Days;
}
