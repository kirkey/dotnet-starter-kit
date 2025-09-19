namespace Store.Domain.Events;

/// <summary>
/// Event raised when a sales order item is added.
/// </summary>
public record SalesOrderItemAdded(
    DefaultIdType Id,
    DefaultIdType SalesOrderId,
    DefaultIdType GroceryItemId,
    string ItemName,
    decimal OrderQuantity,
    decimal UnitPrice,
    decimal LineTotal,
    decimal? DiscountAmount) : DomainEvent;

/// <summary>
/// Event raised when a sales order item is updated.
/// </summary>
public record SalesOrderItemUpdated(Store.Domain.SalesOrderItem SalesOrderItem) : DomainEvent;

/// <summary>
/// Event raised when a sales order item quantity is adjusted.
/// </summary>
public record SalesOrderItemQuantityAdjusted(
    DefaultIdType Id,
    DefaultIdType SalesOrderId,
    DefaultIdType GroceryItemId,
    decimal OldQuantity,
    decimal NewQuantity,
    decimal NewLineTotal) : DomainEvent;
