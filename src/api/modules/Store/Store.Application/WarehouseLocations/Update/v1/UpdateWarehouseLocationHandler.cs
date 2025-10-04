namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Update.v1;

public sealed class UpdateWarehouseLocationHandler(
    ILogger<UpdateWarehouseLocationHandler> logger,
    [FromKeyedServices("store:warehouse-locations")] IRepository<WarehouseLocation> repository,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> warehouseRepository)
    : IRequestHandler<UpdateWarehouseLocationCommand, UpdateWarehouseLocationResponse>
{
    public async Task<UpdateWarehouseLocationResponse> Handle(UpdateWarehouseLocationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var warehouseLocation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = warehouseLocation ?? throw new WarehouseLocationNotFoundException(request.Id);
        
        // Verify warehouse exists if changed
        if (warehouseLocation.WarehouseId != request.WarehouseId)
        {
            var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken).ConfigureAwait(false);
            _ = warehouse ?? throw new WarehouseNotFoundException(request.WarehouseId);
        }

        // Enforce business rule: cannot deactivate a location that contains inventory or used capacity
        if (warehouseLocation.IsActive && !request.IsActive)
        {
            if (!warehouseLocation.CanBeDeactivated())
            {
                throw new WarehouseLocationDeactivationNotAllowedException(warehouseLocation.Id, warehouseLocation.UsedCapacity);
            }
        }
        
        var updatedWarehouseLocation = warehouseLocation.Update(
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
            request.TemperatureUnit,
            request.Notes);

        // If temperature settings changed, use the domain helper to validate and event
        if (request.RequiresTemperatureControl)
        {
            updatedWarehouseLocation.UpdateTemperatureSettings(request.RequiresTemperatureControl, request.MinTemperature, request.MaxTemperature, request.TemperatureUnit);
        }
            
        await repository.UpdateAsync(updatedWarehouseLocation, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("warehouse location with id : {WarehouseLocationId} updated.", warehouseLocation.Id);
        return new UpdateWarehouseLocationResponse(warehouseLocation.Id);
    }
}
