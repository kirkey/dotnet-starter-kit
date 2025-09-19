namespace Store.Domain.Exceptions.SalesOrderItem;

/// <summary>
/// Exception thrown when ID does not find a sales order item.
/// </summary>
public sealed class SalesOrderItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"sales order item with id {id} not found");

/// <summary>
/// Exception thrown when sales order item quantity is invalid.
/// </summary>
public sealed class InvalidSalesOrderItemQuantityException() : ForbiddenException("sales order item quantity must be positive");

/// <summary>
/// Exception thrown when sales order item unit price is invalid.
/// </summary>
public sealed class InvalidSalesOrderItemPriceException() : ForbiddenException("sales order item unit price cannot be negative");

/// <summary>
/// Exception thrown when trying to add a duplicate item to a sales order.
/// </summary>
public sealed class DuplicateSalesOrderItemException(DefaultIdType salesOrderId, DefaultIdType groceryItemId) : ConflictException($"grocery item {groceryItemId} already exists in sales order {salesOrderId}");

/// <summary>
/// Exception thrown when trying to modify a confirmed sales order item.
/// </summary>
public sealed class CannotModifyConfirmedSalesOrderItemException(DefaultIdType id) : ForbiddenException($"cannot modify confirmed sales order item with id {id}");

/// <summary>
/// Exception thrown when discount amount exceeds item total.
/// </summary>
public sealed class DiscountExceedsItemTotalException(decimal itemTotal, decimal discountAmount) 
    : ForbiddenException($"discount amount {discountAmount:C} exceeds item total {itemTotal:C}");

/// <summary>
/// Exception thrown when trying to ship more than ordered quantity.
/// </summary>
public sealed class ShippedQuantityExceedsOrderedException(decimal orderedQuantity, decimal shippedQuantity) 
    : ForbiddenException($"shipped quantity {shippedQuantity} exceeds ordered quantity {orderedQuantity}");
