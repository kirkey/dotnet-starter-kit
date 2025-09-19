namespace Store.Domain.Exceptions.PurchaseOrderItem;

/// <summary>
/// Exception thrown when a purchase order item is not found by ID.
/// </summary>
public sealed class PurchaseOrderItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"purchase order item with id {id} not found");

/// <summary>
/// Exception thrown when purchase order item quantity is invalid.
/// </summary>
public sealed class InvalidPurchaseOrderItemQuantityException() : ForbiddenException("purchase order item quantity must be positive");

/// <summary>
/// Exception thrown when purchase order item unit cost is invalid.
/// </summary>
public sealed class InvalidPurchaseOrderItemCostException() : ForbiddenException("purchase order item unit cost cannot be negative");

/// <summary>
/// Exception thrown when trying to add a duplicate item to a purchase order.
/// </summary>
public sealed class DuplicatePurchaseOrderItemException(DefaultIdType purchaseOrderId, DefaultIdType groceryItemId) : ConflictException($"grocery item {groceryItemId} already exists in purchase order {purchaseOrderId}");

/// <summary>
/// Exception thrown when trying to modify a submitted purchase order item.
/// </summary>
public sealed class CannotModifySubmittedPurchaseOrderItemException(DefaultIdType id) : ForbiddenException($"cannot modify submitted purchase order item with id {id}");

/// <summary>
/// Exception thrown when expected delivery date is invalid.
/// </summary>
public sealed class InvalidExpectedDeliveryDateException() : ForbiddenException("expected delivery date cannot be in the past");
