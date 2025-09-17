namespace Store.Domain;

/// <summary>
/// Represents a transfer of inventory between warehouses or locations.
/// Tracks items, status and timing for the shipment.
/// </summary>
/// <remarks>
/// Use cases:
/// - Move stock from central warehouse to store locations.
/// - Track transfer lifecycle: Pending -> Approved -> InTransit -> Completed.
/// </remarks>
public sealed class InventoryTransfer : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Optional human-readable reason for the transfer.
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Unique transfer number. Example: "TRF-2025-0001".
    /// </summary>
    public string TransferNumber { get; private set; } = default!;

    /// <summary>
    /// Source warehouse identifier.
    /// </summary>
    public DefaultIdType FromWarehouseId { get; private set; }

    /// <summary>
    /// Destination warehouse identifier.
    /// </summary>
    public DefaultIdType ToWarehouseId { get; private set; }

    /// <summary>
    /// Date the transfer was created/sent.
    /// </summary>
    public DateTime TransferDate { get; private set; }

    /// <summary>
    /// Current status: Pending, Approved, InTransit, Completed, Cancelled.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Total monetary value of items included in this transfer.
    /// </summary>
    public decimal TotalValue { get; private set; }

    /// <summary>
    /// Optional transport method (e.g., truck, plane) for the transfer.
    /// </summary>
    public string? TransportMethod { get; private set; }

    /// <summary>
    /// Optional tracking number for the transfer shipment.
    /// </summary>
    public string? TrackingNumber { get; private set; }

    /// <summary>
    /// Optional identifier for the person who requested the transfer.
    /// </summary>
    public string? RequestedBy { get; private set; }

    /// <summary>
    /// Optional identifier for the person who approved the transfer.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Optional date when the transfer was approved.
    /// </summary>
    public DateTime? ApprovalDate { get; private set; }

    /// <summary>
    /// Collection of items included in the transfer.
    /// </summary>
    public ICollection<InventoryTransferItem> Items { get; private set; } = new List<InventoryTransferItem>();

    /// <summary>
    /// The source warehouse for this transfer.
    /// </summary>
    public Warehouse FromWarehouse { get; private set; } = default!;

    /// <summary>
    /// The destination warehouse for this transfer.
    /// </summary>
    public Warehouse ToWarehouse { get; private set; } = default!;

    /// <summary>
    /// Optional source location within the warehouse.
    /// </summary>
    public WarehouseLocation? FromLocation { get; private set; }

    /// <summary>
    /// Optional destination location within the warehouse.
    /// </summary>
    public WarehouseLocation? ToLocation { get; private set; }

    /// <summary>
    /// Optional source location id within the source warehouse (aisle/rack/bin).
    /// </summary>
    public DefaultIdType? FromLocationId { get; private set; }

    /// <summary>
    /// Optional destination location id within the destination warehouse.
    /// </summary>
    public DefaultIdType? ToLocationId { get; private set; }

    /// <summary>
    /// Optional expected arrival date for the transfer shipment.
    /// </summary>
    public DateTime? ExpectedArrivalDate { get; private set; }

    /// <summary>
    /// Type of transfer (e.g., Standard, Express).
    /// </summary>
    public string TransferType { get; private set; } = default!;

    /// <summary>
    /// Priority of the transfer (Low, Normal, High, Critical).
    /// </summary>
    public string Priority { get; private set; } = default!;

    /// <summary>
    /// Actual arrival date when the transfer was completed (set on Complete()).
    /// </summary>
    public DateTime? ActualArrivalDate { get; private set; }

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
        // validations
        if (string.IsNullOrWhiteSpace(transferNumber)) throw new ArgumentException("TransferNumber is required", nameof(transferNumber));
        if (fromWarehouseId == default) throw new ArgumentException("FromWarehouseId is required", nameof(fromWarehouseId));
        if (toWarehouseId == default) throw new ArgumentException("ToWarehouseId is required", nameof(toWarehouseId));
        if (fromWarehouseId == toWarehouseId) throw new ArgumentException("From and To warehouses must be different", nameof(toWarehouseId));
        if (transferDate == default) throw new ArgumentException("TransferDate is required", nameof(transferDate));
        if (string.IsNullOrWhiteSpace(transferType)) throw new ArgumentException("TransferType is required", nameof(transferType));
        if (string.IsNullOrWhiteSpace(priority)) throw new ArgumentException("Priority is required", nameof(priority));

        Id = id;
        TransferNumber = transferNumber;
        FromWarehouseId = fromWarehouseId;
        ToWarehouseId = toWarehouseId;
        FromLocationId = fromLocationId;
        ToLocationId = toLocationId;
        // ensure navigation setters are used (assigned null) to avoid analyzer warnings
        FromLocation = null;
        ToLocation = null;
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
