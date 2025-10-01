namespace Store.Domain.Events;

/// <summary>
/// Event raised when a POS sale item is updated.
/// </summary>
public record PosSaleItemUpdated(PosSaleItem PosSaleItem) : DomainEvent;

/// <summary>
/// Event raised when a POS sale item is removed.
/// </summary>
public record PosSaleItemRemoved(
    DefaultIdType Id,
    DefaultIdType PosSaleId,
    DefaultIdType GroceryItemId,
    decimal Quantity,
    decimal LineTotal) : DomainEvent;

/// <summary>
/// Event raised when a POS sale item quantity is adjusted.
/// </summary>
public record PosSaleItemQuantityAdjusted(
    DefaultIdType Id,
    DefaultIdType PosSaleId,
    DefaultIdType GroceryItemId,
    decimal OldQuantity,
    decimal NewQuantity,
    decimal NewLineTotal) : DomainEvent;

/// <summary>
/// Event raised when a discount is applied to a POS sale item.
/// </summary>
public record PosSaleItemDiscountApplied(
    DefaultIdType Id,
    DefaultIdType PosSaleId,
    DefaultIdType GroceryItemId,
    decimal DiscountAmount,
    string? DiscountReason) : DomainEvent;
