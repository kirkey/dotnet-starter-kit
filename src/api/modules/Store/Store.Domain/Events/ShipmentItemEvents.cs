namespace Store.Domain.Events;

/// <summary>
/// Event raised when a shipment item is updated.
/// </summary>
public record ShipmentItemUpdated(ShipmentItem ShipmentItem) : DomainEvent;

/// <summary>
/// Event raised when a shipment item is removed.
/// </summary>
public record ShipmentItemRemoved(
    DefaultIdType Id,
    DefaultIdType ShipmentId,
    DefaultIdType GroceryItemId,
    decimal ShippedQuantity) : DomainEvent;

/// <summary>
/// Event raised when a shipment item quantity is adjusted.
/// </summary>
public record ShipmentItemQuantityAdjusted(
    DefaultIdType Id,
    DefaultIdType ShipmentId,
    DefaultIdType GroceryItemId,
    decimal OldQuantity,
    decimal NewQuantity) : DomainEvent;
