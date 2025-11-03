namespace Store.Domain.Entities;

/// <summary>
/// Represents a put-away task for storing received inventory into warehouse locations after goods receipt.
/// </summary>
/// <remarks>
/// Use cases:
/// - Direct received goods to optimal storage locations based on item characteristics.
/// - Support directed put-away strategies (ABC analysis, velocity-based).
/// - Track put-away completion and worker productivity.
/// - Enable cross-docking for immediate outbound shipments.
/// - Support zone-based put-away assignments.
/// - Track put-away time for performance metrics.
/// 
/// Default values:
/// - TaskNumber: required unique identifier (example: "PUT-2025-001")
/// - WarehouseId: required warehouse location
/// - Status: "Created" (Created, Assigned, InProgress, Completed, Cancelled)
/// - Priority: 0 (higher number = higher priority)
/// - GoodsReceiptId: optional source receipt reference
/// 
/// Business rules:
/// - TaskNumber must be unique
/// - Cannot modify completed tasks
/// - Put-away updates stock levels and bin utilization
/// - Items must match receipt quantities
/// - Directed put-away must respect bin capacity
/// </remarks>
/// <seealso cref="Store.Domain.Events.PutAwayTaskCreated"/>
/// <seealso cref="Store.Domain.Events.PutAwayTaskCompleted"/>
/// <seealso cref="Store.Domain.Exceptions.PutAwayTask.PutAwayTaskNotFoundException"/>
public sealed class PutAwayTask : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique put-away task number.
    /// Example: "PUT-2025-001", "PA-WH1-001".
    /// Max length: 100.
    /// </summary>
    public string TaskNumber { get; private set; } = default!;

    /// <summary>
    /// Warehouse where put-away occurs.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Optional source goods receipt.
    /// </summary>
    public DefaultIdType? GoodsReceiptId { get; private set; }

    /// <summary>
    /// Task status: Created, Assigned, InProgress, Completed, Cancelled.
    /// </summary>
    public string Status { get; private set; } = "Created";

    /// <summary>
    /// Priority (higher number = higher priority).
    /// Example: 10 for perishables, 1 for standard.
    /// </summary>
    public int Priority { get; private set; }

    /// <summary>
    /// User/employee assigned to perform put-away.
    /// Max length: 100.
    /// </summary>
    public string? AssignedTo { get; private set; }

    /// <summary>
    /// Date/time when put-away started.
    /// </summary>
    public DateTime? StartDate { get; private set; }

    /// <summary>
    /// Date/time when put-away completed.
    /// </summary>
    public DateTime? CompletedDate { get; private set; }

    /// <summary>
    /// Put-away strategy: Standard, ABC, CrossDock, Directed.
    /// Max length: 50.
    /// </summary>
    public string? PutAwayStrategy { get; private set; }

    /// <summary>
    /// Optional notes or special instructions.
    /// Max length: 500.
    /// </summary>

    /// <summary>
    /// Total lines on put-away task.
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
    /// Navigation property to goods receipt.
    /// </summary>
    public GoodsReceipt? GoodsReceipt { get; private set; }

    private readonly List<PutAwayTaskItem> _items = new();
    /// <summary>
    /// Collection of put-away task items, each representing an item to be stored in a specific location.
    /// Read-only to enforce proper aggregate management.
    /// </summary>
    public IReadOnlyCollection<PutAwayTaskItem> Items => _items.AsReadOnly();

    private PutAwayTask() { }

    private PutAwayTask(
        DefaultIdType id,
        string taskNumber,
        DefaultIdType warehouseId,
        DefaultIdType? goodsReceiptId,
        int priority,
        string? putAwayStrategy,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(taskNumber)) throw new ArgumentException("TaskNumber is required", nameof(taskNumber));
        if (taskNumber.Length > 100) throw new ArgumentException("TaskNumber must not exceed 100 characters", nameof(taskNumber));

        if (warehouseId == DefaultIdType.Empty) throw new ArgumentException("WarehouseId is required", nameof(warehouseId));

        if (putAwayStrategy is { Length: > 50 }) throw new ArgumentException("PutAwayStrategy must not exceed 50 characters", nameof(putAwayStrategy));
        if (notes is { Length: > 500 }) throw new ArgumentException("Notes must not exceed 500 characters", nameof(notes));

        Id = id;
        TaskNumber = taskNumber;
        WarehouseId = warehouseId;
        GoodsReceiptId = goodsReceiptId;
        Priority = priority;
        PutAwayStrategy = putAwayStrategy ?? "Standard";
        Notes = notes;
        Status = "Created";
        TotalLines = 0;
        CompletedLines = 0;

        QueueDomainEvent(new PutAwayTaskCreated { PutAwayTask = this });
    }

    public static PutAwayTask Create(
        string taskNumber,
        DefaultIdType warehouseId,
        DefaultIdType? goodsReceiptId = null,
        int priority = 0,
        string? putAwayStrategy = null,
        string? notes = null)
    {
        return new PutAwayTask(
            DefaultIdType.NewGuid(),
            taskNumber,
            warehouseId,
            goodsReceiptId,
            priority,
            putAwayStrategy,
            notes);
    }

    public PutAwayTask AddItem(
        DefaultIdType itemId,
        DefaultIdType toBinId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId,
        int quantity,
        int sequenceNumber,
        string? notes = null)
    {
        if (Status != "Created") throw new InvalidOperationException("Cannot add items to put-away task after it has been assigned");

        var taskItem = PutAwayTaskItem.Create(Id, itemId, toBinId, lotNumberId, serialNumberId, quantity, notes);
        taskItem.SetSequence(sequenceNumber);
        _items.Add(taskItem);
        TotalLines++;

        QueueDomainEvent(new PutAwayTaskItemAdded { PutAwayTask = this, Item = taskItem });
        return this;
    }

    public PutAwayTask AssignToWorker(string workerUserId)
    {
        if (string.IsNullOrWhiteSpace(workerUserId)) throw new ArgumentException("Worker user ID is required", nameof(workerUserId));
        if (Status != "Created") throw new InvalidOperationException($"Cannot assign put-away task in {Status} status");

        AssignedTo = workerUserId;
        Status = "Assigned";

        QueueDomainEvent(new PutAwayTaskAssigned { PutAwayTask = this, AssignedTo = workerUserId });
        return this;
    }

    public PutAwayTask StartPutAway()
    {
        if (Status != "Assigned") throw new InvalidOperationException($"Cannot start put-away from {Status} status");
        if (string.IsNullOrWhiteSpace(AssignedTo)) throw new InvalidOperationException("Put-away task must be assigned before starting");

        Status = "InProgress";
        StartDate = DateTime.UtcNow;

        QueueDomainEvent(new PutAwayTaskStarted { PutAwayTask = this });
        return this;
    }

    public PutAwayTask CompletePutAway()
    {
        if (Status != "InProgress") throw new InvalidOperationException($"Cannot complete put-away task in {Status} status");

        Status = "Completed";
        CompletedDate = DateTime.UtcNow;

        QueueDomainEvent(new PutAwayTaskCompleted { PutAwayTask = this });
        return this;
    }

    public PutAwayTask IncrementCompletedLines()
    {
        CompletedLines++;
        QueueDomainEvent(new PutAwayTaskUpdated { PutAwayTask = this });
        return this;
    }

    public decimal GetCompletionPercentage() =>
        TotalLines > 0 ? (decimal)CompletedLines / TotalLines * 100 : 0;
}
