using FSH.Framework.Core.Domain.Events;

namespace Store.Domain.Events;

public record StockAdjustmentCreated : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = default!;
}

public record StockAdjustmentApproved : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = default!;
}

public record StockAdjustmentUpdated : DomainEvent
{
    public StockAdjustment StockAdjustment { get; init; } = default!;
}

