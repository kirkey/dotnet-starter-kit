namespace Store.Domain.Events;

public record PriceListCreated : DomainEvent
{
    public PriceList PriceList { get; init; } = default!;
}

public record PriceListUpdated : DomainEvent
{
    public PriceList PriceList { get; init; } = default!;
}

public record PriceListActivated : DomainEvent
{
    public PriceList PriceList { get; init; } = default!;
}

public record PriceListDeactivated : DomainEvent
{
    public PriceList PriceList { get; init; } = default!;
}

// PriceListItem domain events
public record PriceListItemCreated : DomainEvent
{
    public PriceListItem PriceListItem { get; init; } = default!;
}

public record PriceListItemUpdated : DomainEvent
{
    public PriceListItem PriceListItem { get; init; } = default!;
}
