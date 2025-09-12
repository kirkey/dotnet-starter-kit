using FSH.Framework.Core.Domain.Events;

namespace Store.Domain.Events;

public record CycleCountCreated : DomainEvent
{
    public CycleCount CycleCount { get; init; } = default!;
}

public record CycleCountStarted : DomainEvent
{
    public CycleCount CycleCount { get; init; } = default!;
}

public record CycleCountCompleted : DomainEvent
{
    public CycleCount CycleCount { get; init; } = default!;
}

public record CycleCountItemCreated : DomainEvent
{
    public CycleCountItem CycleCountItem { get; init; } = default!;
}

public record CycleCountItemCounted : DomainEvent
{
    public CycleCountItem CycleCountItem { get; init; } = default!;
}

public record CycleCountItemMarkedForRecount : DomainEvent
{
    public CycleCountItem CycleCountItem { get; init; } = default!;
    public string Reason { get; init; } = default!;
}

public record PriceListCreated : DomainEvent
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

public record PriceListItemCreated : DomainEvent
{
    public PriceListItem PriceListItem { get; init; } = default!;
}

public record PriceListItemUpdated : DomainEvent
{
    public PriceListItem PriceListItem { get; init; } = default!;
}
