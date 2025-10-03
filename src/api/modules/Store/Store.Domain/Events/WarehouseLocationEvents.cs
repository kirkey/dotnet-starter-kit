using Store.Domain.Entities;

namespace Store.Domain.Events;

public record WarehouseLocationCreated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}

public record WarehouseLocationUpdated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}

public record WarehouseLocationCapacityUpdated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
    public decimal PreviousUsage { get; init; }
    public decimal NewUsage { get; init; }
}

public record WarehouseLocationActivated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}

public record WarehouseLocationDeactivated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}
