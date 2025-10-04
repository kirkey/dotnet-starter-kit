namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Update.v1;

public record UpdateWarehouseLocationCommand(
    DefaultIdType Id,
    [property: DefaultValue("Main Location")] string Name,
    [property: DefaultValue("Primary storage location")] string? Description,
    [property: DefaultValue("LOC001")] string Code,
    [property: DefaultValue("A")] string Aisle,
    [property: DefaultValue("01")] string Section,
    [property: DefaultValue("01")] string Shelf,
    [property: DefaultValue("A")] string? Bin,
    DefaultIdType WarehouseId,
    [property: DefaultValue("Floor")] string LocationType,
    [property: DefaultValue(1000)] decimal Capacity,
    [property: DefaultValue("sqft")] string CapacityUnit,
    [property: DefaultValue(true)] bool IsActive,
    [property: DefaultValue(false)] bool RequiresTemperatureControl,
    [property: DefaultValue(null)] decimal? MinTemperature,
    [property: DefaultValue(null)] decimal? MaxTemperature,
    [property: DefaultValue("C")] string? TemperatureUnit,
    string? Notes) : IRequest<UpdateWarehouseLocationResponse>;
