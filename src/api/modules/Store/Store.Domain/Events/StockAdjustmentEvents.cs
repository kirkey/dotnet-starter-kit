using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new stock adjustment is created.
/// </summary>
public record StockAdjustmentCreated : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = null!;
}

/// <summary>
/// Event raised when a stock adjustment is approved.
/// </summary>
public record StockAdjustmentApproved : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = null!;
    public string ApprovedBy { get; init; } = null!;
    public DateTime ApprovalDate { get; init; }
}

/// <summary>
/// Event raised when a stock adjustment is updated.
/// </summary>
public record StockAdjustmentUpdated : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = null!;
}

/// <summary>
/// Event raised when a stock adjustment is cancelled.
/// </summary>
public record StockAdjustmentCancelled : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = null!;
    public string Reason { get; init; } = null!;
}

/// <summary>
/// Event raised when a stock adjustment is rejected.
/// </summary>
public record StockAdjustmentRejected : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = null!;
    public string RejectedBy { get; init; } = null!;
    public string Reason { get; init; } = null!;
}

