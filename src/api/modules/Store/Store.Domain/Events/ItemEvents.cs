using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new item is created in the system.
/// </summary>
public record ItemCreated : DomainEvent
{
    public Item Item { get; init; } = default!;
}

/// <summary>
/// Event raised when an item is updated.
/// </summary>
public record ItemUpdated : DomainEvent
{
    public Item Item { get; init; } = default!;
}

/// <summary>
/// Event raised when an item's price is changed.
/// </summary>
public record ItemPriceChanged : DomainEvent
{
    public Item Item { get; init; } = default!;
    public decimal OldPrice { get; init; }
    public decimal NewPrice { get; init; }
}

/// <summary>
/// Event raised when an item reaches its reorder point.
/// </summary>
public record ItemReorderPointReached : DomainEvent
{
    public Item Item { get; init; } = default!;
    public int CurrentStock { get; init; }
    public int ReorderPoint { get; init; }
}

/// <summary>
/// Event raised when a perishable item is expiring soon.
/// </summary>
public record ItemExpiring : DomainEvent
{
    public Item Item { get; init; } = default!;
    public DateTime ExpiryDate { get; init; }
    public int DaysRemaining { get; init; }
}

/// <summary>
/// Event raised when an item is activated.
/// </summary>
public record ItemActivated : DomainEvent
{
    public Item Item { get; init; } = default!;
}

/// <summary>
/// Event raised when an item is deactivated.
/// </summary>
public record ItemDeactivated : DomainEvent
{
    public Item Item { get; init; } = default!;
}

/// <summary>
/// Event raised when an item is deleted.
/// </summary>
public record ItemDeleted : DomainEvent
{
    public DefaultIdType ItemId { get; init; }
    public string Sku { get; init; } = default!;
}
