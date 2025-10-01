namespace Store.Domain.Events;

public record CustomerReturnCreated : DomainEvent { public CustomerReturn CustomerReturn { get; init; } = default!; }
public record CustomerReturnItemAdded : DomainEvent { public CustomerReturn CustomerReturn { get; init; } = default!; public CustomerReturnItem Item { get; init; } = default!; }

