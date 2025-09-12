


namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Create.v1;

public sealed class CreateWarehouseLocationHandler(
    ILogger<CreateWarehouseLocationHandler> logger,
    [FromKeyedServices("store:warehouse-locations")] IRepository<WarehouseLocation> repository,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> warehouseRepository)
    : IRequestHandler<CreateWarehouseLocationCommand, CreateWarehouseLocationResponse>
{
    public async Task<CreateWarehouseLocationResponse> Handle(CreateWarehouseLocationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        // Verify warehouse exists
        var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken).ConfigureAwait(false);
        _ = warehouse ?? throw new WarehouseNotFoundException(request.WarehouseId);
        
        var warehouseLocation = WarehouseLocation.Create(
            request.Name,
            request.Description,
            request.Code,
            request.Aisle,
            request.Section,
            request.Shelf,
            request.Bin,
            request.WarehouseId,
            request.LocationType,
            request.Capacity,
            request.CapacityUnit,
            request.IsActive,
            request.RequiresTemperatureControl,
            request.MinTemperature,
            request.MaxTemperature,
            request.TemperatureUnit);
            
        await repository.AddAsync(warehouseLocation, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("warehouse location created {WarehouseLocationId}", warehouseLocation.Id);
        return new CreateWarehouseLocationResponse(warehouseLocation.Id);
    }
}
