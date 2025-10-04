namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;

public record GetWarehouseLocationListResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
    string? Notes,
    string Code,
    string Aisle,
    string Section,
    string Shelf,
    string? Bin,
    DefaultIdType WarehouseId,
    string WarehouseName,
    string LocationType,
    decimal Capacity,
    decimal UsedCapacity,
    string CapacityUnit,
    bool IsActive,
    bool RequiresTemperatureControl);
