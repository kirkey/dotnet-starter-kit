namespace Store.Domain.Exceptions.PurchaseOrderItem;

/// <summary>
/// Exception thrown when a received quantity exceeds the ordered quantity for a purchase order item.
/// </summary>
public sealed class ReceivedQuantityExceedsOrderedException(DefaultIdType id)
    : ForbiddenException($"received quantity exceeds ordered quantity for purchase order item {id}") {}

