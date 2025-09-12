using FSH.Framework.Core.Domain.Events;

namespace Store.Domain.Events;

public record CustomerCreated : DomainEvent
{
    public Customer Customer { get; init; } = default!;
}

public record CustomerUpdated : DomainEvent
{
    public Customer Customer { get; init; } = default!;
}

public record CustomerBalanceUpdated : DomainEvent
{
    public Customer Customer { get; init; } = default!;
    public decimal PreviousBalance { get; init; }
    public decimal NewBalance { get; init; }
    public string Operation { get; init; } = default!;
    public decimal Amount { get; init; }
}

public record CustomerLifetimeValueUpdated : DomainEvent
{
    public Customer Customer { get; init; } = default!;
    public decimal AdditionalValue { get; init; }
}

