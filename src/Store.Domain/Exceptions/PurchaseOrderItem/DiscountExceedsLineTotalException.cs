namespace Store.Domain.Exceptions.PurchaseOrderItem;

/// <summary>
/// Exception thrown when a discount exceeds the line total (quantity * unit price) for a purchase order item.
/// </summary>
public sealed class DiscountExceedsLineTotalException(DefaultIdType id)
    : ForbiddenException($"discount exceeds line total for purchase order item {id}") {}

