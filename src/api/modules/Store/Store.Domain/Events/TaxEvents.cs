namespace Store.Domain.Events;

public record TaxRateCreated : DomainEvent { public TaxRate TaxRate { get; init; } = default!; }
public record TaxRateUpdated : DomainEvent { public TaxRate TaxRate { get; init; } = default!; }

