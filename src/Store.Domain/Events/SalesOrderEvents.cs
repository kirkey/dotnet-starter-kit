using FSH.Framework.Core.Domain.Events;

namespace Store.Domain.Events;

public record SalesOrderCreated : DomainEvent
{
    public SalesOrder SalesOrder { get; init; } = default!;
}

public record SalesOrderUpdated : DomainEvent
{
    public SalesOrder SalesOrder { get; init; } = default!;
}

public record SalesOrderItemCreated : DomainEvent
{
    public SalesOrderItem SalesOrderItem { get; init; } = default!;
}

public record SalesOrderItemQuantityUpdated : DomainEvent
{
    public SalesOrderItem SalesOrderItem { get; init; } = default!;
}

public record SalesOrderItemShipped : DomainEvent
{
    public SalesOrderItem SalesOrderItem { get; init; } = default!;
    public int PreviousShippedQuantity { get; init; }
    public int NewShippedQuantity { get; init; }
}

public record SalesOrderStatusChanged : DomainEvent
{
    public SalesOrder SalesOrder { get; init; } = default!;
    public string PreviousStatus { get; init; } = default!;
    public string NewStatus { get; init; } = default!;
}

public record SalesOrderPaymentStatusChanged : DomainEvent
{
    public SalesOrder SalesOrder { get; init; } = default!;
    public string PreviousStatus { get; init; } = default!;
    public string NewStatus { get; init; } = default!;
}

public record SalesOrderDiscountApplied : DomainEvent
{
    public SalesOrder SalesOrder { get; init; } = default!;
    public decimal DiscountAmount { get; init; }
}

public record SalesOrderDeliveryScheduled : DomainEvent
{
    public SalesOrder SalesOrder { get; init; } = default!;
}

