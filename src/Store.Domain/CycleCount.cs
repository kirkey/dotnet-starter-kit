namespace Store.Domain;

public sealed class CycleCount : AuditableEntity, IAggregateRoot
{
    public string CountNumber { get; private set; } = default!;
    public DefaultIdType WarehouseId { get; private set; }
    public DefaultIdType? WarehouseLocationId { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public DateTime? ActualStartDate { get; private set; }
    public DateTime? CompletionDate { get; private set; }
    public string Status { get; private set; } = default!; // Scheduled, InProgress, Completed, Cancelled
    public string CountType { get; private set; } = default!; // Full, Partial, ABC, Random
    public string? CounterName { get; private set; }
    public string? SupervisorName { get; private set; }
    public int TotalItemsToCount { get; private set; }
    public int ItemsCountedCorrect { get; private set; }
    public int ItemsWithDiscrepancies { get; private set; }
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
