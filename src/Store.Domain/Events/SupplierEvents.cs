namespace Store.Domain.Events;

public record SupplierCreated : DomainEvent
{
    public Supplier Supplier { get; init; } = default!;
}

public record SupplierUpdated : DomainEvent
{
    public Supplier Supplier { get; init; } = default!;
}

public record SupplierActivated : DomainEvent
{
    public Supplier Supplier { get; init; } = default!;
}

public record SupplierDeactivated : DomainEvent
{
    public Supplier Supplier { get; init; } = default!;
}
