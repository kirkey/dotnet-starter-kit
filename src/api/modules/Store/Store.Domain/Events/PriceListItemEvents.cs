namespace Store.Domain.Events;

/// <summary>
/// Event raised when a price list item is added.
/// </summary>
public record PriceListItemAdded(
    DefaultIdType Id,
    DefaultIdType PriceListId,
    DefaultIdType GroceryItemId,
    string ItemName,
    decimal Price,
    DateTime EffectiveDate,
    DateTime? ExpirationDate) : DomainEvent;

/// <summary>
/// Event raised when a price list item is removed.
/// </summary>
public record PriceListItemRemoved(
    DefaultIdType Id,
    DefaultIdType PriceListId,
    DefaultIdType GroceryItemId,
    decimal Price) : DomainEvent;

/// <summary>
/// Event raised when a price list item price is changed.
/// </summary>
public record PriceListItemPriceChanged(
    DefaultIdType Id,
    DefaultIdType PriceListId,
    DefaultIdType GroceryItemId,
    decimal OldPrice,
    decimal NewPrice,
    DateTime EffectiveDate) : DomainEvent;
