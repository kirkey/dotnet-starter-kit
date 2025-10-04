using Store.Domain.Entities;

namespace Store.Domain.Events;

/// <summary>
/// Event raised when a new warehouse location is created.
/// </summary>
public record WarehouseLocationCreated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}

/// <summary>
/// Event raised when a warehouse location is updated.
/// </summary>
public record WarehouseLocationUpdated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}

/// <summary>
/// Event raised when warehouse location capacity utilization is updated.
/// </summary>
public record WarehouseLocationCapacityUpdated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
    public decimal PreviousUsage { get; init; }
    public decimal NewUsage { get; init; }
}

/// <summary>
/// Event raised when a warehouse location is activated.
/// </summary>
public record WarehouseLocationActivated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}

/// <summary>
/// Event raised when a warehouse location is deactivated.
/// </summary>
public record WarehouseLocationDeactivated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
}

/// <summary>
/// Event raised when temperature settings are updated for a warehouse location.
/// </summary>
public record WarehouseLocationTemperatureSettingsUpdated : DomainEvent
{
    public WarehouseLocation WarehouseLocation { get; init; } = default!;
    public bool RequiresTemperatureControl { get; init; }
    public decimal? MinTemperature { get; init; }
    public decimal? MaxTemperature { get; init; }
}

