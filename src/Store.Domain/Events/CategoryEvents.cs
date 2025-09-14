namespace Store.Domain.Events;

public record CategoryCreated : DomainEvent
{
    public Category Category { get; init; } = default!;
}

public record CategoryUpdated : DomainEvent
{
    public Category Category { get; init; } = default!;
}

public record CategoryActivated : DomainEvent
{
    public Category Category { get; init; } = default!;
}

public record CategoryDeactivated : DomainEvent
{
    public Category Category { get; init; } = default!;
}
