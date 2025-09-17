namespace Store.Domain;

/// <summary>
/// Represents a scheduled inventory cycle count in a warehouse.
/// Use cycle counts to verify stock levels without doing a full inventory.
/// </summary>
/// <remarks>
/// Use cases:
/// - Schedule partial or full counts to find discrepancies.
/// - Track progress (Scheduled, InProgress, Completed).
/// - Attach counted items (<see cref="Items"/>) produced by counting activities.
/// </remarks>
public sealed class CycleCount : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Human-friendly identifier for the count. Example: "CC-2025-09-001".
    /// </summary>
    public string CountNumber { get; private set; } = default!;

    /// <summary>
    /// Warehouse id where the count is taking place.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Optional location within the warehouse (aisle/rack) being counted.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Date the count is scheduled to run. Required.
    /// </summary>
    public DateTime ScheduledDate { get; private set; }

    /// <summary>
    /// When the count actually started (set when started). null until started.
    /// </summary>
    public DateTime? ActualStartDate { get; private set; }

    /// <summary>
    /// When the count was completed. null until completed.
    /// </summary>
    public DateTime? CompletionDate { get; private set; }

    /// <summary>
    /// Status of the count (Scheduled, InProgress, Completed, Cancelled). Default: "Scheduled".
    /// </summary>
    public string Status { get; private set; } = default!; // Scheduled, InProgress, Completed, Cancelled

    /// <summary>
    /// Type of count (Full, Partial, ABC, Random). Use to describe scope.
    /// </summary>
    public string CountType { get; private set; } = default!; // Full, Partial, ABC, Random

    /// <summary>
    /// Name of the person who is counting (optional).
    /// </summary>
    public string? CounterName { get; private set; }

    /// <summary>
    /// Supervisor name for the count (optional).
    /// </summary>
    public string? SupervisorName { get; private set; }

    /// <summary>
    /// Totals for reporting: items to count, correct counts, discrepancies.
    /// </summary>
    public int TotalItemsToCount { get; private set; }
    public int ItemsCountedCorrect { get; private set; }
    public int ItemsWithDiscrepancies { get; private set; }

    /// <summary>
    /// Accuracy percentage computed after completion. Range 0-100.
    /// </summary>
    public decimal AccuracyPercentage { get; private set; }
    
    
    public Warehouse Warehouse { get; private set; } = default!;
    public WarehouseLocation? WarehouseLocation { get; private set; }
    public ICollection<CycleCountItem> Items { get; private set; } = new List<CycleCountItem>();

    private CycleCount() { }

    private CycleCount(
        DefaultIdType id,
        string countNumber,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        DateTime scheduledDate,
        string countType,
        string? counterName,
        string? supervisorName,
        string? notes)
    {
        // validations
        if (string.IsNullOrWhiteSpace(countNumber)) throw new ArgumentException("CountNumber is required", nameof(countNumber));
        if (countNumber.Length > 100) throw new ArgumentException("CountNumber must not exceed 100 characters", nameof(countNumber));

        if (warehouseId == default) throw new ArgumentException("WarehouseId is required", nameof(warehouseId));

        if (scheduledDate == default) throw new ArgumentException("ScheduledDate is required", nameof(scheduledDate));

        if (string.IsNullOrWhiteSpace(countType)) throw new ArgumentException("CountType is required", nameof(countType));
        if (countType.Length > 50) throw new ArgumentException("CountType must not exceed 50 characters", nameof(countType));

        if (counterName is { Length: > 100 }) throw new ArgumentException("CounterName must not exceed 100 characters", nameof(counterName));
        if (supervisorName is { Length: > 100 }) throw new ArgumentException("SupervisorName must not exceed 100 characters", nameof(supervisorName));

        Id = id;
        CountNumber = countNumber;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        ScheduledDate = scheduledDate;
        Status = "Scheduled";
        CountType = countType;
        CounterName = counterName;
        SupervisorName = supervisorName;
        TotalItemsToCount = 0;
        ItemsCountedCorrect = 0;
        ItemsWithDiscrepancies = 0;
        AccuracyPercentage = 0;
        Notes = notes;

        QueueDomainEvent(new CycleCountCreated { CycleCount = this });
    }

    public static CycleCount Create(
        string countNumber,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        DateTime scheduledDate,
        string countType,
        string? counterName = null,
        string? supervisorName = null,
        string? notes = null)
    {
        return new CycleCount(
            DefaultIdType.NewGuid(),
            countNumber,
            warehouseId,
            warehouseLocationId,
            scheduledDate,
            countType,
            counterName,
            supervisorName,
            notes);
    }

    public CycleCount Start()
    {
        if (Status == "Scheduled")
        {
            Status = "InProgress";
            ActualStartDate = DateTime.UtcNow;
            QueueDomainEvent(new CycleCountStarted { CycleCount = this });
        }
        return this;
    }

    public CycleCount Complete()
    {
        if (Status == "InProgress")
        {
            Status = "Completed";
            CompletionDate = DateTime.UtcNow;
            CalculateAccuracy();
            QueueDomainEvent(new CycleCountCompleted { CycleCount = this });
        }
        return this;
    }

    public CycleCount AddItem(DefaultIdType groceryItemId, int systemQuantity, int? countedQuantity = null)
    {
        var item = CycleCountItem.Create(Id, groceryItemId, systemQuantity, countedQuantity);
        Items.Add(item);
        UpdateCounts();
        return this;
    }

    private void CalculateAccuracy()
    {
        TotalItemsToCount = Items.Count;
        ItemsCountedCorrect = Items.Count(i => i.IsAccurate());
        ItemsWithDiscrepancies = TotalItemsToCount - ItemsCountedCorrect;
        AccuracyPercentage = TotalItemsToCount > 0 ? (decimal)ItemsCountedCorrect / TotalItemsToCount * 100 : 100;
    }

    private void UpdateCounts()
    {
        TotalItemsToCount = Items.Count;
        ItemsCountedCorrect = Items.Count(i => i.IsAccurate());
        ItemsWithDiscrepancies = TotalItemsToCount - ItemsCountedCorrect;
    }

    public bool IsOverdue() => Status == "Scheduled" && DateTime.UtcNow > ScheduledDate;
    public bool HasHighAccuracy(decimal threshold = 95) => AccuracyPercentage >= threshold;
}
