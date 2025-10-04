namespace Store.Domain.Exceptions.CustomerReturnItem;

/// <summary>
/// Exception thrown when a customer return item is not found by ID.
/// </summary>
public sealed class CustomerReturnItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"customer return item with id {id} not found");

/// <summary>
/// Exception thrown when a customer return item quantity is invalid.
/// </summary>
public sealed class InvalidCustomerReturnItemQuantityException() : ForbiddenException("customer return item quantity must be positive");

/// <summary>
/// Exception thrown when trying to return more quantity than originally purchased.
/// </summary>
public sealed class ReturnQuantityExceedsPurchasedException(decimal purchasedQuantity, decimal returnQuantity) 
    : ForbiddenException($"return quantity {returnQuantity} exceeds purchased quantity {purchasedQuantity}");

/// <summary>
/// Exception thrown when customer return item unit price is invalid.
/// </summary>
public sealed class InvalidCustomerReturnItemPriceException() : ForbiddenException("customer return item unit price cannot be negative");

/// <summary>
/// Exception thrown when trying to modify a processed customer return item.
/// </summary>
public sealed class CannotModifyProcessedReturnItemException(DefaultIdType id) : ForbiddenException($"cannot modify processed customer return item with id {id}");

/// <summary>
/// Exception thrown when grocery item is not returnable.
/// </summary>
public sealed class ItemNotReturnableException(DefaultIdType itemId) : ForbiddenException($"item with id {itemId} is not returnable");
