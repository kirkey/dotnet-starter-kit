using Store.Domain.Exceptions.CycleCount;

namespace Store.Domain.Entities;

/// <summary>
/// Represents a scheduled inventory cycle count in a warehouse with comprehensive tracking and variance analysis.
/// </summary>
/// <remarks>
/// Use cases:
/// - Schedule regular cycle counts to maintain inventory accuracy without full physical counts.
/// - Track count progress from scheduled through completed with status management.
/// - Identify and resolve inventory discrepancies between system and physical counts.
/// - Support ABC analysis by counting high-value items more frequently than low-value items.
/// - Enable perpetual inventory management with continuous counting cycles.
/// - Generate variance reports for inventory adjustments and process improvements.
/// - Support audit requirements for inventory accuracy and control procedures.
/// - Track counter performance and accuracy metrics for training purposes.
/// 
/// Default values:
/// - CountNumber: required unique identifier (example: "CC-2025-09-001")
/// - WarehouseId: required warehouse reference for count location
/// - WarehouseLocationId: optional specific location (example: aisle, rack, bin)
/// - ScheduledDate: required scheduled count date (example: 2025-09-19)
/// - Status: "Scheduled" (new counts start as scheduled)
/// - CountType: required count classification (example: "ABC", "Full", "Spot")
/// - StartedDate: null (set when counting begins)
/// - CompletedDate: null (set when count is finished)
/// - CountedBy: null (set to counter identifier when started)
/// - TotalVariance: 0.00 (calculated from count discrepancies)
/// - ItemsPlanned: 0 (number of items scheduled for counting)
/// - ItemsCounted: 0 (number of items actually counted)
/// 
/// Business rules:
/// - CountNumber must be unique within the system
/// - Cannot modify completed counts without proper authorization
/// - Scheduled date cannot be in the past for new counts
/// - Cannot start count without proper counter assignment
/// - Count completion requires all planned items to be counted or noted as missing
/// - Variance calculations must account for system vs physical quantities
/// - Count adjustments require approval above specified variance thresholds
/// - Counter assignment requires active employee status
/// - Location must be within the specified warehouse
/// </remarks>
/// <seealso cref="Store.Domain.Events.CycleCountCreated"/>
/// <seealso cref="Store.Domain.Events.CycleCountUpdated"/>
/// <seealso cref="Store.Domain.Events.CycleCountStarted"/>
/// <seealso cref="Store.Domain.Events.CycleCountCompleted"/>
/// <seealso cref="Store.Domain.Events.CycleCountVarianceDetected"/>
/// <seealso cref="Store.Domain.Events.CycleCountCancelled"/>
/// <seealso cref="Store.Domain.Exceptions.CycleCount.CycleCountNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.CycleCount.CycleCountCannotBeModifiedException"/>
/// <seealso cref="Store.Domain.Exceptions.CycleCount.InvalidCycleCountStatusException"/>
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
    
    private readonly List<CycleCountItem> _items = new();
    /// <summary>
    /// Collection of cycle count items, each representing a counted item with system vs physical quantities.
    /// Read-only to enforce proper aggregate management.
    /// </summary>
    public IReadOnlyCollection<CycleCountItem> Items => _items.AsReadOnly();

    // Parameterless constructor for EF Core
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

    /// <summary>
    /// Updates cycle count details. Only allowed for cycle counts in 'Scheduled' status.
    /// </summary>
    /// <param name="warehouseId">The warehouse where the count will take place.</param>
    /// <param name="warehouseLocationId">Optional specific location within the warehouse.</param>
    /// <param name="scheduledDate">The scheduled date for the count.</param>
    /// <param name="countType">The type of count (Full, Partial, ABC, Random).</param>
    /// <param name="description">Optional description of the cycle count.</param>
    /// <param name="counterName">Optional name of the person who will perform the count.</param>
    /// <param name="supervisorName">Optional name of the supervisor.</param>
    /// <param name="notes">Optional notes about the count.</param>
    /// <returns>The updated CycleCount.</returns>
    /// <exception cref="Store.Domain.Exceptions.CycleCount.CycleCountCannotBeModifiedException">Thrown when attempting to modify a cycle count that is not in 'Scheduled' status.</exception>
    public CycleCount Update(
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        DateTime scheduledDate,
        string countType,
        string? description,
        string? counterName,
        string? supervisorName,
        string? notes)
    {
        if (Status != "Scheduled")
        {
            throw new CycleCountCannotBeModifiedException(Id, Status);
        }

        // Validations
        if (warehouseId == default) throw new ArgumentException("WarehouseId is required", nameof(warehouseId));
        if (scheduledDate == default) throw new ArgumentException("ScheduledDate is required", nameof(scheduledDate));
        if (string.IsNullOrWhiteSpace(countType)) throw new ArgumentException("CountType is required", nameof(countType));
        if (countType.Length > 50) throw new ArgumentException("CountType must not exceed 50 characters", nameof(countType));
        if (description is { Length: > 2048 }) throw new ArgumentException("Description must not exceed 2048 characters", nameof(description));
        if (counterName is { Length: > 100 }) throw new ArgumentException("CounterName must not exceed 100 characters", nameof(counterName));
        if (supervisorName is { Length: > 100 }) throw new ArgumentException("SupervisorName must not exceed 100 characters", nameof(supervisorName));

        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        ScheduledDate = scheduledDate;
        CountType = countType;
        Description = description;
        CounterName = counterName;
        SupervisorName = supervisorName;
        Notes = notes;

        QueueDomainEvent(new CycleCountUpdated { CycleCount = this });

        return this;
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

    /// <summary>
    /// Cancels the cycle count.
    /// </summary>
    /// <param name="reason">The reason for cancellation.</param>
    /// <returns>The updated CycleCount.</returns>
    public CycleCount Cancel(string reason)
    {
        if (Status == "Scheduled" || Status == "InProgress")
        {
            Status = "Cancelled";
            Notes = string.IsNullOrWhiteSpace(Notes) 
                ? $"Cancelled: {reason}" 
                : $"{Notes}\nCancelled: {reason}";
            QueueDomainEvent(new CycleCountCancelled { CycleCount = this, Reason = reason });
        }
        return this;
    }

    /// <summary>
    /// Update the aggregate counts after items have changed. 
    /// Intended to be called by application handlers managing CycleCountItem entities.
    /// Similar to Budget.SetTotals() for consistency.
    /// </summary>
    /// <param name="totalItems">Total number of items in the count.</param>
    /// <param name="itemsCountedCorrect">Number of items counted correctly (no variance).</param>
    /// <param name="itemsWithDiscrepancies">Number of items with variances.</param>
    public CycleCount SetCounts(int totalItems, int itemsCountedCorrect, int itemsWithDiscrepancies)
    {
        TotalItemsToCount = totalItems;
        ItemsCountedCorrect = itemsCountedCorrect;
        ItemsWithDiscrepancies = itemsWithDiscrepancies;
        AccuracyPercentage = totalItems > 0 ? (decimal)itemsCountedCorrect / totalItems * 100 : 100;
        return this;
    }

    private void CalculateAccuracy()
    {
        TotalItemsToCount = _items.Count;
        ItemsCountedCorrect = _items.Count(i => i.IsAccurate());
        ItemsWithDiscrepancies = TotalItemsToCount - ItemsCountedCorrect;
        AccuracyPercentage = TotalItemsToCount > 0 ? (decimal)ItemsCountedCorrect / TotalItemsToCount * 100 : 100;
    }

    private void UpdateCounts()
    {
        TotalItemsToCount = _items.Count;
        ItemsCountedCorrect = _items.Count(i => i.IsAccurate());
        ItemsWithDiscrepancies = TotalItemsToCount - ItemsCountedCorrect;
    }

    public bool IsOverdue() => Status == "Scheduled" && DateTime.UtcNow > ScheduledDate;
    public bool HasHighAccuracy(decimal threshold = 95) => AccuracyPercentage >= threshold;
}
