namespace Store.Domain.Entities;

/// <summary>
/// Represents a picking list for warehouse order fulfillment with tasks for workers to collect items from storage locations.
/// </summary>
/// <remarks>
/// Use cases:
/// - Generate pick lists for order fulfillment with optimized pick paths.
/// - Support batch picking for multiple orders simultaneously.
/// - Track picker assignment and completion status.
/// - Enable wave picking strategies for efficient warehouse operations.
/// - Support zone picking with multiple pickers per wave.
/// - Track picking accuracy and productivity metrics.
/// - Generate exception reports for short picks and substitutions.
/// 
/// Default values:
/// - PickListNumber: required unique identifier (example: "PICK-2025-001")
/// - WarehouseId: required warehouse where picking occurs
/// - Status: "Created" (Created, Assigned, InProgress, Completed, Cancelled)
/// - PickingType: required type (Order, Wave, Batch, Zone)
/// - Priority: 0 (higher number = higher priority)
/// - AssignedTo: null (picker assignment)
/// - StartDate: null (when picking starts)
/// - CompletedDate: null (when picking completes)
/// 
/// Business rules:
/// - PickListNumber must be unique
/// - Cannot modify completed pick lists
/// - Assigned picker must be active employee
/// - All items must be allocated before picking
/// - Short picks require supervisor approval
/// - Picking updates stock levels in real-time
/// </remarks>
/// <seealso cref="Store.Domain.Events.PickListCreated"/>
/// <seealso cref="Store.Domain.Events.PickListAssigned"/>
/// <seealso cref="Store.Domain.Events.PickListCompleted"/>
/// <seealso cref="Store.Domain.Exceptions.PickList.PickListNotFoundException"/>
public sealed class PickList : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique pick list number.
    /// Example: "PICK-2025-001", "WAVE-001-ZONE-A".
    /// Max length: 100.
    /// </summary>
    public string PickListNumber { get; private set; } = default!;

    /// <summary>
    /// Warehouse where picking occurs.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Pick list status: Created, Assigned, InProgress, Completed, Cancelled.
    /// </summary>
    public string Status { get; private set; } = "Created";

    /// <summary>
    /// Type of picking: Order, Wave, Batch, Zone.
    /// </summary>
    public string PickingType { get; private set; } = default!;

    /// <summary>
    /// Priority (higher number = higher priority).
    /// Example: 10 for rush orders, 1 for standard.
    /// </summary>
    public int Priority { get; private set; }

    /// <summary>
    /// User/employee assigned to pick this list.
    /// Example: "john.smith", "PICKER-001".
    /// Max length: 100.
    /// </summary>
    public string? AssignedTo { get; private set; }

    /// <summary>
    /// Date/time when picking started.
    /// </summary>
    public DateTime? StartDate { get; private set; }

    /// <summary>
    /// Date/time when picking completed.
    /// </summary>
    public DateTime? CompletedDate { get; private set; }

    /// <summary>
    /// Expected completion time for performance tracking.
    /// </summary>
    public DateTime? ExpectedCompletionDate { get; private set; }

    /// <summary>
    /// Optional reference to source document (order number, wave number).
    /// Max length: 100.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Optional notes or special instructions.
    /// Max length: 500.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Total lines on pick list.
    /// </summary>
    public int TotalLines { get; private set; }

    /// <summary>
    /// Completed lines.
    /// </summary>
    public int CompletedLines { get; private set; }

    /// <summary>
    /// Navigation property to warehouse.
    /// </summary>
    public Warehouse Warehouse { get; private set; } = default!;

    /// <summary>
    /// Pick list items to be picked.
    /// </summary>
    public ICollection<PickListItem> Items { get; private set; } = new List<PickListItem>();

    private PickList() { }

    private PickList(
        DefaultIdType id,
        string pickListNumber,
        DefaultIdType warehouseId,
        string pickingType,
        int priority,
        string? referenceNumber,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(pickListNumber)) throw new ArgumentException("PickListNumber is required", nameof(pickListNumber));
        if (pickListNumber.Length > 100) throw new ArgumentException("PickListNumber must not exceed 100 characters", nameof(pickListNumber));

        if (warehouseId == DefaultIdType.Empty) throw new ArgumentException("WarehouseId is required", nameof(warehouseId));

        if (string.IsNullOrWhiteSpace(pickingType)) throw new ArgumentException("PickingType is required", nameof(pickingType));
        var validTypes = new[] { "Order", "Wave", "Batch", "Zone" };
        if (!validTypes.Contains(pickingType, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"PickingType must be one of: {string.Join(", ", validTypes)}", nameof(pickingType));

        if (referenceNumber is { Length: > 100 }) throw new ArgumentException("ReferenceNumber must not exceed 100 characters", nameof(referenceNumber));
        if (notes is { Length: > 500 }) throw new ArgumentException("Notes must not exceed 500 characters", nameof(notes));

        Id = id;
        PickListNumber = pickListNumber;
        WarehouseId = warehouseId;
        PickingType = pickingType;
        Priority = priority;
        ReferenceNumber = referenceNumber;
        Notes = notes;
        Status = "Created";
        TotalLines = 0;
        CompletedLines = 0;

        QueueDomainEvent(new PickListCreated { PickList = this });
    }

    public static PickList Create(
        string pickListNumber,
        DefaultIdType warehouseId,
        string pickingType,
        int priority = 0,
        string? referenceNumber = null,
        string? notes = null)
    {
        return new PickList(
            DefaultIdType.NewGuid(),
            pickListNumber,
            warehouseId,
            pickingType,
            priority,
            referenceNumber,
            notes);
    }

    public PickList AddItem(
        DefaultIdType itemId,
        DefaultIdType? binId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId,
        int quantityToPick,
        string? notes = null)
    {
        if (Status != "Created") throw new InvalidOperationException("Cannot add items to pick list after it has been assigned");

        var pickListItem = PickListItem.Create(Id, itemId, binId, lotNumberId, serialNumberId, quantityToPick, notes);
        Items.Add(pickListItem);
        TotalLines++;

        QueueDomainEvent(new PickListItemAdded { PickList = this, Item = pickListItem });
        return this;
    }

    public PickList AssignToPicker(string pickerUserId)
    {
        if (string.IsNullOrWhiteSpace(pickerUserId)) throw new ArgumentException("Picker user ID is required", nameof(pickerUserId));
        if (Status != "Created") throw new InvalidOperationException($"Cannot assign pick list in {Status} status");

        AssignedTo = pickerUserId;
        Status = "Assigned";

        QueueDomainEvent(new PickListAssigned { PickList = this, AssignedTo = pickerUserId });
        return this;
    }

    public PickList StartPicking()
    {
        if (Status != "Assigned") throw new InvalidOperationException($"Cannot start picking from {Status} status");
        if (string.IsNullOrWhiteSpace(AssignedTo)) throw new InvalidOperationException("Pick list must be assigned before starting");

        Status = "InProgress";
        StartDate = DateTime.UtcNow;

        QueueDomainEvent(new PickListStarted { PickList = this });
        return this;
    }

    public PickList CompletePicking()
    {
        if (Status != "InProgress") throw new InvalidOperationException($"Cannot complete pick list in {Status} status");

        Status = "Completed";
        CompletedDate = DateTime.UtcNow;

        QueueDomainEvent(new PickListCompleted { PickList = this });
        return this;
    }

    public PickList Cancel(string reason)
    {
        if (Status == "Completed") throw new InvalidOperationException("Cannot cancel completed pick list");

        Status = "Cancelled";
        Notes = string.IsNullOrWhiteSpace(Notes) ? reason : $"{Notes}\nCancelled: {reason}";

        QueueDomainEvent(new PickListCancelled { PickList = this, Reason = reason });
        return this;
    }

    public PickList IncrementCompletedLines()
    {
        CompletedLines++;
        QueueDomainEvent(new PickListUpdated { PickList = this });
        return this;
    }

    public decimal GetCompletionPercentage() =>
        TotalLines > 0 ? (decimal)CompletedLines / TotalLines * 100 : 0;
}
