using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new cycle count is created.
/// </summary>
public record CycleCountCreated : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
}

/// <summary>
/// Event raised when a cycle count is updated.
/// </summary>
public record CycleCountUpdated : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
}

/// <summary>
/// Event raised when a cycle count is started.
/// </summary>
public record CycleCountStarted : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
}

/// <summary>
/// Event raised when a cycle count is completed.
/// </summary>
public record CycleCountCompleted : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
}

/// <summary>
/// Event raised when a cycle count is cancelled.
/// </summary>
public record CycleCountCancelled : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
    public string Reason { get; init; } = null!;
}

/// <summary>
/// Event raised when a variance is detected during cycle count.
/// </summary>
public record CycleCountVarianceDetected : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
    public DefaultIdType ItemId { get; init; }
    public int SystemQuantity { get; init; }
    public int CountedQuantity { get; init; }
    public int Variance { get; init; }
}

/// <summary>
/// Event raised when an item is added to a cycle count.
/// </summary>
public record CycleCountItemAdded : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
    public CycleCountItem Item { get; init; } = null!;
}

/// <summary>
/// Event raised when accuracy is calculated for a cycle count.
/// </summary>
public record CycleCountAccuracyCalculated : DomainEvent
{
    public CycleCount CycleCount { get; init; } = null!;
    public decimal AccuracyPercentage { get; init; }
    public int TotalItems { get; init; }
    public int CorrectItems { get; init; }
    public int DiscrepancyItems { get; init; }
}

public record CycleCountItemCreated : DomainEvent
{
    public CycleCountItem CycleCountItem { get; init; } = null!;
}

public record CycleCountItemCounted : DomainEvent
{
    public CycleCountItem CycleCountItem { get; init; } = null!;
}

public record CycleCountItemMarkedForRecount : DomainEvent
{
    public CycleCountItem CycleCountItem { get; init; } = null!;
    public string Reason { get; init; } = null!;
}
