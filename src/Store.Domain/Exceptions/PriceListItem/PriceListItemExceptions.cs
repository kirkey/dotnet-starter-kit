namespace Store.Domain.Exceptions.PriceListItem;

/// <summary>
/// Exception thrown when a price list item is not found by ID.
/// </summary>
public sealed class PriceListItemByIdNotFoundException(DefaultIdType id) : NotFoundException($"price list item with id {id} not found");

/// <summary>
/// Exception thrown when price list item price is invalid.
/// </summary>
public sealed class InvalidPriceListItemPriceException() : ForbiddenException("price list item price cannot be negative");

/// <summary>
/// Exception thrown when trying to add a duplicate item to a price list.
/// </summary>
public sealed class DuplicatePriceListItemException(DefaultIdType priceListId, DefaultIdType groceryItemId) : ConflictException($"grocery item {groceryItemId} already exists in price list {priceListId}");

/// <summary>
/// Exception thrown when effective date is invalid.
/// </summary>
public sealed class InvalidEffectiveDateException() : ForbiddenException("effective date cannot be in the past");

/// <summary>
/// Exception thrown when expiration date is before effective date.
/// </summary>
public sealed class InvalidExpirationDateException() : ForbiddenException("expiration date cannot be before effective date");

/// <summary>
/// Exception thrown when trying to modify an active price list item.
/// </summary>
public sealed class CannotModifyActivePriceListItemException(DefaultIdType id) : ForbiddenException($"cannot modify active price list item with id {id}");
