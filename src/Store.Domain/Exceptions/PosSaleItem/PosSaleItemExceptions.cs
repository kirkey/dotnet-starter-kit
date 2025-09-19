namespace Store.Domain.Exceptions.PosSaleItem;

/// <summary>
/// Exception thrown when ID does not find a POS sale item.
/// </summary>
public sealed class PosSaleItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"POS sale item with id {id} not found");

/// <summary>
/// Exception thrown when POS sale item quantity is invalid.
/// </summary>
public sealed class InvalidPosSaleItemQuantityException() : ForbiddenException("POS sale item quantity must be positive");

/// <summary>
/// Exception thrown when POS sale item unit price is invalid.
/// </summary>
public sealed class InvalidPosSaleItemPriceException() : ForbiddenException("POS sale item unit price cannot be negative");

/// <summary>
/// Exception thrown when trying to sell more than available inventory.
/// </summary>
public sealed class InsufficientInventoryForSaleException(decimal availableQuantity, decimal requestedQuantity) 
    : ForbiddenException($"insufficient inventory. Available: {availableQuantity}, Requested: {requestedQuantity}");

/// <summary>
/// Exception thrown when trying to modify a finalized POS sale item.
/// </summary>
public sealed class CannotModifyFinalizedPosSaleItemException(DefaultIdType id) : ForbiddenException($"cannot modify finalized POS sale item with id {id}");

/// <summary>
/// Exception thrown when discount amount exceeds item total.
/// </summary>
public sealed class DiscountExceedsItemTotalException(decimal itemTotal, decimal discountAmount) 
    : ForbiddenException($"discount amount {discountAmount:C} exceeds item total {itemTotal:C}");

/// <summary>
/// Exception thrown when trying to add a duplicate item to a POS sale.
/// </summary>
public sealed class DuplicatePosSaleItemException(DefaultIdType posSaleId, DefaultIdType groceryItemId) : ConflictException($"grocery item {groceryItemId} already exists in POS sale {posSaleId}");
