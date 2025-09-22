namespace Store.Domain.Exceptions.PurchaseOrderItem;

/// <summary>
/// Exception thrown when attempting to reduce a purchase order item quantity below the already received quantity.
/// </summary>
public sealed class CannotReduceQuantityBelowReceivedException(DefaultIdType id)
    : ForbiddenException($"cannot reduce quantity below received amount for purchase order item {id}") {}

