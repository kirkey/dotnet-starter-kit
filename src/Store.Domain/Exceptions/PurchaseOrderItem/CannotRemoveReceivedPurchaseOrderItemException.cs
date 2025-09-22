namespace Store.Domain.Exceptions.PurchaseOrderItem;

/// <summary>
/// Exception thrown when attempting to remove a purchase order item that already has received quantity.
/// </summary>
public sealed class CannotRemoveReceivedPurchaseOrderItemException(DefaultIdType id)
    : ForbiddenException($"cannot remove purchase order item {id} because it already has received quantity") {}

