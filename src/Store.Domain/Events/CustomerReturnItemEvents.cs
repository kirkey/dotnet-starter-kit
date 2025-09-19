namespace Store.Domain.Events;

/// <summary>
/// Event raised when a customer return item is updated.
/// </summary>
public record CustomerReturnItemUpdated(Store.Domain.CustomerReturnItem CustomerReturnItem) : DomainEvent;

/// <summary>
/// Event raised when a customer return item is removed.
/// </summary>
public record CustomerReturnItemRemoved(
    DefaultIdType Id,
    DefaultIdType CustomerReturnId,
    DefaultIdType GroceryItemId,
    decimal Quantity) : DomainEvent;

/// <summary>
/// Event raised when a customer return item quantity is adjusted.
/// </summary>
public record CustomerReturnItemQuantityAdjusted(
    DefaultIdType Id,
    DefaultIdType CustomerReturnId,
    DefaultIdType GroceryItemId,
    decimal OldQuantity,
    decimal NewQuantity,
    decimal NewLineTotal) : DomainEvent;
