namespace Store.Domain.Events;

/// <summary>
/// Raised when a new grocery item is created.
/// </summary>
public record GroceryItemCreated : DomainEvent
{
    /// <summary>
    /// The created grocery item aggregate.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;
}

/// <summary>
/// Raised when a grocery item is updated.
/// </summary>
public record GroceryItemUpdated : DomainEvent
{
    /// <summary>
    /// The updated grocery item aggregate.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;
}

/// <summary>
/// Raised when a grocery item's stock changes.
/// </summary>
public record GroceryItemStockUpdated : DomainEvent
{
    /// <summary>
    /// The grocery item whose stock has changed.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;

    /// <summary>
    /// The stock level before the change.
    /// </summary>
    public int PreviousStock { get; init; }

    /// <summary>
    /// The stock level after the change.
    /// </summary>
    public int NewStock { get; init; }

    /// <summary>
    /// The stock operation applied (ADD, REMOVE, SET).
    /// </summary>
    public string Operation { get; init; } = default!;

    /// <summary>
    /// The absolute quantity changed.
    /// </summary>
    public int Quantity { get; init; }
}

/// <summary>
/// Raised when a grocery item's stock-related thresholds are updated.
/// </summary>
public record GroceryItemStockLevelsUpdated : DomainEvent
{
    /// <summary>
    /// The grocery item whose stock parameters changed.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;
}

/// <summary>
/// Raised when a grocery item is assigned to a new warehouse location.
/// </summary>
public record GroceryItemLocationAssigned : DomainEvent
{
    /// <summary>
    /// The grocery item assigned to a location.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;
}

/// <summary>
/// Raised when a grocery item's price changes.
/// </summary>
public record GroceryItemPriceChanged : DomainEvent
{
    /// <summary>
    /// The grocery item whose price changed.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;

    /// <summary>
    /// The previous price.
    /// </summary>
    public decimal OldPrice { get; init; }

    /// <summary>
    /// The new price.
    /// </summary>
    public decimal NewPrice { get; init; }
}

/// <summary>
/// Raised when a grocery item's stock falls to or below the reorder point.
/// </summary>
public record GroceryItemReorderPointReached : DomainEvent
{
    /// <summary>
    /// The grocery item needing reorder.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;

    /// <summary>
    /// Current stock when threshold was reached.
    /// </summary>
    public int CurrentStock { get; init; }

    /// <summary>
    /// The reorder point threshold.
    /// </summary>
    public int ReorderPoint { get; init; }
}

/// <summary>
/// Raised when a perishable grocery item is nearing its expiry date.
/// </summary>
public record GroceryItemExpiring : DomainEvent
{
    /// <summary>
    /// The grocery item nearing expiry.
    /// </summary>
    public GroceryItem GroceryItem { get; init; } = default!;

    /// <summary>
    /// The expiry date of the item.
    /// </summary>
    public DateTime ExpiryDate { get; init; }

    /// <summary>
    /// Number of days remaining until expiry.
    /// </summary>
    public int DaysRemaining { get; init; }
}
