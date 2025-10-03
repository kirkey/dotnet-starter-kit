using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a purchase order item is updated.
/// </summary>
public record PurchaseOrderItemUpdated(PurchaseOrderItem PurchaseOrderItem) : DomainEvent;

/// <summary>
/// Event raised when a purchase order item quantity is adjusted.
/// </summary>
public record PurchaseOrderItemQuantityAdjusted(
    DefaultIdType Id,
    DefaultIdType PurchaseOrderId,
    DefaultIdType ItemId,
    decimal OldQuantity,
    decimal NewQuantity,
    decimal NewLineTotal) : DomainEvent;
