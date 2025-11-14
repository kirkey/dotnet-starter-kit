using Store.Domain.Entities;

namespace Store.Domain.Events;

public record WarehouseCreated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = null!;
}

public record WarehouseUpdated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = null!;
}

public record WarehouseCapacityUpdated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = null!;
    public decimal PreviousUsage { get; init; }
    public decimal NewUsage { get; init; }
}

public record WarehouseInventoryCounted : DomainEvent
{
    public Warehouse Warehouse { get; init; } = null!;
    public DateTime InventoryDate { get; init; }
}

public record WarehouseActivated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = null!;
}

public record WarehouseDeactivated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = null!;
}

public record WarehouseManagerAssigned : DomainEvent
{
    public Warehouse Warehouse { get; init; } = null!;
    public string PreviousManagerName { get; init; } = null!;
    public string NewManagerName { get; init; } = null!;
}
