namespace Store.Domain.Events;

public record GroceryItemCreated : DomainEvent
{
    public GroceryItem GroceryItem { get; init; } = default!;
}

public record GroceryItemUpdated : DomainEvent
{
    public GroceryItem GroceryItem { get; init; } = default!;
}

public record GroceryItemStockUpdated : DomainEvent
{
    public GroceryItem GroceryItem { get; init; } = default!;
    public int PreviousStock { get; init; }
    public int NewStock { get; init; }
    public string Operation { get; init; } = default!;
    public int Quantity { get; init; }
}

public record GroceryItemStockLevelsUpdated : DomainEvent
{
    public GroceryItem GroceryItem { get; init; } = default!;
}

public record GroceryItemLocationAssigned : DomainEvent
{
    public GroceryItem GroceryItem { get; init; } = default!;
}
