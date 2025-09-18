using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Specs;

namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Get.v1;

public sealed class GetWarehouseLocationHandler(
    ILogger<GetWarehouseLocationHandler> logger,
    [FromKeyedServices("store:warehouse-locations")] IRepository<WarehouseLocation> repository)
    : IRequestHandler<GetWarehouseLocationQuery, GetWarehouseLocationResponse>
{
    public async Task<GetWarehouseLocationResponse> Handle(GetWarehouseLocationQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var spec = new GetWarehouseLocationSpecification(request.Id);
        var warehouseLocation = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);
        _ = warehouseLocation ?? throw new WarehouseLocationNotFoundException(request.Id);
        
        logger.LogInformation("Retrieved warehouse location {WarehouseLocationId}", warehouseLocation.Id);
        
        return new GetWarehouseLocationResponse(
            warehouseLocation.Id,
            warehouseLocation.Name!,
            warehouseLocation.Description,
            warehouseLocation.Code,
            warehouseLocation.Aisle,
            warehouseLocation.Section,
            warehouseLocation.Shelf,
            warehouseLocation.Bin,
            warehouseLocation.WarehouseId,
            warehouseLocation.Warehouse.Name!,
            warehouseLocation.LocationType,
            warehouseLocation.Capacity,
            warehouseLocation.UsedCapacity,
            warehouseLocation.CapacityUnit,
            warehouseLocation.IsActive,
            warehouseLocation.RequiresTemperatureControl,
            warehouseLocation.MinTemperature,
            warehouseLocation.MaxTemperature,
            warehouseLocation.TemperatureUnit,
            warehouseLocation.CreatedOn.LocalDateTime,
            warehouseLocation.LastModifiedOn.LocalDateTime);
    }
}
