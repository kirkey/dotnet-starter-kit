namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Get.v1;

public record GetWarehouseLocationResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
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
    bool RequiresTemperatureControl,
    decimal? MinTemperature,
    decimal? MaxTemperature,
    string? TemperatureUnit,
    DateTime CreatedOn,
    DateTime? LastModifiedOn);
