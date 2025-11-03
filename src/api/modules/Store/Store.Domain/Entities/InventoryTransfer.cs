namespace Store.Domain.Entities;

/// <summary>
/// Represents an inventory transfer between warehouses or locations with comprehensive tracking and approval workflows.
/// </summary>
/// <remarks>
/// Use cases:
/// - Transfer inventory between central warehouse and store locations for replenishment.
/// - Move stock from overstocked locations to understocked locations for optimization.
/// - Support inter-company transfers between different business units or entities.
/// - Track transfer lifecycle with approval workflows (Pending → Approved → InTransit → Completed).
/// - Handle emergency stock transfers for critical inventory shortages.
/// - Support seasonal inventory redistribution and store closures/openings.
/// - Enable inventory balancing across multiple distribution centers.
/// - Track transfer costs including transportation and handling expenses.
/// 
/// Default values:
/// - TransferNumber: required unique identifier (example: "TRF-2025-09-001")
/// - FromWarehouseId: required source warehouse reference
/// - ToWarehouseId: required destination warehouse reference
/// - Status: "Pending" (new transfers start pending approval)
/// - TransferDate: required transfer initiation date (example: 2025-09-19)
/// - ExpectedDate: optional expected completion (example: 2025-09-21)
/// - CompletedDate: null (set when transfer is completed)
/// - Reason: optional transfer justification (example: "Store replenishment", "Emergency stock")
/// - TotalCost: 0.00 (calculated from transportation and handling)
/// - IsUrgent: false (standard priority unless marked urgent)
/// - ApprovedBy: null (set when transfer is approved)
/// - ApprovedDate: null (set when approval occurs)
/// 
/// Business rules:
/// - TransferNumber must be unique within the system
/// - Cannot transfer between the same warehouse (source ≠ destination)
/// - Cannot modify approved transfers without proper authorization
/// - Expected date should be after transfer date when specified
/// - Cannot complete transfers with unresolved discrepancies
/// - Approval required for transfers above specified value thresholds
/// - Source warehouse must have sufficient inventory for transfer
/// - Both warehouses must be active to create transfers
/// - Transfer completion updates inventory balances automatically
/// </remarks>
/// <seealso cref="Store.Domain.Events.InventoryTransferCreated"/>
/// <seealso cref="Store.Domain.Events.InventoryTransferUpdated"/>
/// <seealso cref="Store.Domain.Events.InventoryTransferApproved"/>
/// <seealso cref="Store.Domain.Events.InventoryTransferShipped"/>
/// <seealso cref="Store.Domain.Events.InventoryTransferCompleted"/>
/// <seealso cref="Store.Domain.Events.InventoryTransferCancelled"/>
/// <seealso cref="Store.Domain.Exceptions.InventoryTransfer.InventoryTransferNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.InventoryTransfer.InventoryTransferCannotBeModifiedException"/>
/// <seealso cref="Store.Domain.Exceptions.InventoryTransfer.InvalidInventoryTransferStatusException"/>
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

    private readonly List<InventoryTransferItem> _items = new();
    /// <summary>
    /// Collection of items included in the transfer with quantities and tracking information.
    /// Read-only to enforce proper aggregate management.
    /// </summary>
    public IReadOnlyCollection<InventoryTransferItem> Items => _items.AsReadOnly();

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

    public InventoryTransfer AddItem(DefaultIdType itemId, int quantity, decimal unitPrice)
    {
        if (itemId == default) throw new ArgumentException("ItemId is required", nameof(itemId));
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        // Only allow adding items when transfer is in Pending state.
        if (!string.Equals(Status, "Pending", StringComparison.OrdinalIgnoreCase))
            throw new Store.Domain.Exceptions.InventoryTransfer.InventoryTransferCannotBeModifiedException(Id);

        // Prevent duplicate items in the same transfer.
        if (Items.Any(i => i.ItemId == itemId))
            throw new Store.Domain.Exceptions.InventoryTransferItem.DuplicateInventoryTransferItemException(Id, itemId);

        var item = InventoryTransferItem.Create(Id, itemId, quantity, unitPrice);
        _items.Add(item);
        RecalculateTotal();

        // Raise a specific domain event for item addition
        QueueDomainEvent(new InventoryTransferItemAdded { InventoryTransfer = this, ItemId = itemId });
        return this;
    }

    public InventoryTransfer RemoveItem(DefaultIdType itemId)
    {
        // Only allow removing items when transfer is in Pending state.
        if (!string.Equals(Status, "Pending", StringComparison.OrdinalIgnoreCase))
            throw new Store.Domain.Exceptions.InventoryTransfer.InventoryTransferCannotBeModifiedException(Id);

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            _items.Remove(item);
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
        if (!string.Equals(Status, "Pending", StringComparison.OrdinalIgnoreCase)) return this;
        Status = "Approved";
        ApprovedBy = approvedBy;
        ApprovalDate = DateTime.UtcNow;
        QueueDomainEvent(new InventoryTransferApproved { InventoryTransfer = this });
        return this;
    }

    public InventoryTransfer MarkInTransit()
    {
        if (string.Equals(Status, "Approved", StringComparison.OrdinalIgnoreCase))
        {
            Status = "InTransit";
            QueueDomainEvent(new InventoryTransferInTransit { InventoryTransfer = this });
        }
        return this;
    }

    public InventoryTransfer Complete(DateTime actualArrival)
    {
        if (string.Equals(Status, "InTransit", StringComparison.OrdinalIgnoreCase))
        {
            Status = "Completed";
            ActualArrivalDate = actualArrival;
            QueueDomainEvent(new InventoryTransferCompleted { InventoryTransfer = this });
        }
        return this;
    }

    public InventoryTransfer Cancel()
    {
        return Cancel(null);
    }

    /// <summary>
    /// Cancels the transfer recording an optional reason. Cancellation is allowed unless the transfer is already completed.
    /// </summary>
    public InventoryTransfer Cancel(string? reason)
    {
        if (string.Equals(Status, "Completed", StringComparison.OrdinalIgnoreCase))
            throw new Store.Domain.Exceptions.InventoryTransfer.InvalidInventoryTransferStatusException("Cancel", Status);

        Status = "Cancelled";
        if (!string.IsNullOrWhiteSpace(reason)) Reason = reason;
        QueueDomainEvent(new InventoryTransferCancelled { InventoryTransfer = this, Reason = reason ?? string.Empty });
        return this;
    }

    // New helper to set tracking number (used when shipment/tracking info is available)
    public InventoryTransfer SetTrackingNumber(string? trackingNumber)
    {
        // Only allow setting tracking when transfer is already Approved or InTransit
        if (!string.Equals(Status, "Approved", StringComparison.OrdinalIgnoreCase) && !string.Equals(Status, "InTransit", StringComparison.OrdinalIgnoreCase))
            throw new Store.Domain.Exceptions.InventoryTransfer.InvalidInventoryTransferStatusException("SetTrackingNumber", Status);

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
        string transferType,
        DefaultIdType fromWarehouseId,
        DefaultIdType toWarehouseId,
        DateTime transferDate,
        DateTime? expectedArrivalDate,
        string status,
        string priority,
        string? transportMethod,
        string? trackingNumber,
        string? notes,
        string? reason)
    {
        // Only allow updates while Pending (business rule: no unrestricted edits after approval)
        if (!string.Equals(Status, "Pending", StringComparison.OrdinalIgnoreCase))
            throw new Store.Domain.Exceptions.InventoryTransfer.InventoryTransferCannotBeModifiedException(Id);

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

        if (TransferType != transferType)
        {
            TransferType = transferType;
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

        if (ExpectedArrivalDate != expectedArrivalDate)
        {
            ExpectedArrivalDate = expectedArrivalDate;
            changed = true;
        }

        if (Status != status)
        {
            Status = status;
            changed = true;
        }

        if (Priority != priority)
        {
            Priority = priority;
            changed = true;
        }

        if (TransportMethod != transportMethod)
        {
            TransportMethod = transportMethod;
            changed = true;
        }

        if (TrackingNumber != trackingNumber)
        {
            TrackingNumber = trackingNumber;
            changed = true;
        }

        if (Notes != notes)
        {
            Notes = notes; // inherited
            changed = true;
        }

        if (Reason != reason)
        {
            Reason = reason; // inherited
            changed = true;
        }

        if (changed)
        {
            QueueDomainEvent(new InventoryTransferUpdated { InventoryTransfer = this });
        }

        return this;
    }

    /// <summary>
    /// Updates a specific <see cref="InventoryTransferItem"/> attached to this transfer.
    /// This enforces the business rule that items can only be updated while the transfer is in the <c>Pending</c> state.
    /// After updating the line item the transfer total is recalculated and an <see cref="Store.Domain.Events.InventoryTransferUpdated"/> event is queued.
    /// </summary>
    /// <param name="itemId">The identifier of the transfer item to update.</param>
    /// <param name="quantity">New quantity for the item; must be greater than zero.</param>
    /// <param name="unitPrice">New unit price for the item; must be non-negative.</param>
    /// <returns>The current <see cref="InventoryTransfer"/> instance with updated totals.</returns>
    public InventoryTransfer UpdateItem(DefaultIdType itemId, int quantity, decimal unitPrice)
    {
        if (itemId == default) throw new ArgumentException("ItemId is required", nameof(itemId));

        // Only allow updating items when transfer is in Pending state.
        if (!string.Equals(Status, "Pending", StringComparison.OrdinalIgnoreCase))
            throw new Store.Domain.Exceptions.InventoryTransfer.InventoryTransferCannotBeModifiedException(Id);

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null) throw new Store.Domain.Exceptions.InventoryTransfer.InventoryTransferItemNotFoundException(itemId);

        item.Update(quantity, unitPrice);
        RecalculateTotal();

        // Emit a transfer-level updated event as overall totals changed
        QueueDomainEvent(new InventoryTransferUpdated { InventoryTransfer = this });
        return this;
    }
}
