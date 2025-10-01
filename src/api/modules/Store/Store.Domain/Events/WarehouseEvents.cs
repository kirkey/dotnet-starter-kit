namespace Store.Domain.Events;

public record WarehouseCreated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = default!;
}

public record WarehouseUpdated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = default!;
}

public record WarehouseCapacityUpdated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = default!;
    public decimal PreviousUsage { get; init; }
    public decimal NewUsage { get; init; }
}

public record WarehouseInventoryCounted : DomainEvent
{
    public Warehouse Warehouse { get; init; } = default!;
    public DateTime InventoryDate { get; init; }
}

public record WarehouseActivated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = default!;
}

public record WarehouseDeactivated : DomainEvent
{
    public Warehouse Warehouse { get; init; } = default!;
}

public record WarehouseManagerAssigned : DomainEvent
{
    public Warehouse Warehouse { get; init; } = default!;
    public string PreviousManagerName { get; init; } = default!;
    public string NewManagerName { get; init; } = default!;
}
