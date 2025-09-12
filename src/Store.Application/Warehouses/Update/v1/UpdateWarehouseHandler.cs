


namespace FSH.Starter.WebApi.Store.Application.Warehouses.Update.v1;

public sealed class UpdateWarehouseHandler(
    ILogger<UpdateWarehouseHandler> logger,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> repository)
    : IRequestHandler<UpdateWarehouseCommand, UpdateWarehouseResponse>
{
    public async Task<UpdateWarehouseResponse> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var warehouse = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = warehouse ?? throw new WarehouseNotFoundException(request.Id);
        
        var updatedWarehouse = warehouse.Update(
            request.Name,
            request.Description,
            request.Address,
            request.City,
            request.State,
            request.Country,
            request.PostalCode,
            request.ManagerName,
            request.ManagerEmail,
            request.ManagerPhone,
            request.TotalCapacity,
            request.CapacityUnit,
            request.IsMainWarehouse);
            
        await repository.UpdateAsync(updatedWarehouse, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("warehouse with id : {WarehouseId} updated.", warehouse.Id);
        return new UpdateWarehouseResponse(warehouse.Id);
    }
}
