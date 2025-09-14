namespace Store.Domain.Events;

public record PurchaseOrderCreated : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

public record PurchaseOrderStatusChanged : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    public string PreviousStatus { get; init; } = default!;
    public string NewStatus { get; init; } = default!;
}

public record PurchaseOrderItemAdded : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    public DefaultIdType GroceryItemId { get; init; }
}

public record PurchaseOrderItemRemoved : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    public DefaultIdType GroceryItemId { get; init; }
}

public record PurchaseOrderDelivered : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}

public record PurchaseOrderDiscountApplied : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
    public decimal DiscountAmount { get; init; }
}

public record PurchaseOrderItemCreated : DomainEvent
{
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
}

public record PurchaseOrderItemQuantityUpdated : DomainEvent
{
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
}

public record PurchaseOrderItemPriceUpdated : DomainEvent
{
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
}

public record PurchaseOrderItemReceived : DomainEvent
{
    public PurchaseOrderItem PurchaseOrderItem { get; init; } = default!;
    public int PreviousReceivedQuantity { get; init; }
    public int NewReceivedQuantity { get; init; }
}

public record PurchaseOrderUpdated : DomainEvent
{
    public PurchaseOrder PurchaseOrder { get; init; } = default!;
}
