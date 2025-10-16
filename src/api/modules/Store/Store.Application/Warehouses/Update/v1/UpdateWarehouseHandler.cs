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
            request.Notes,
            request.Address,
            request.ManagerName,
            request.ManagerEmail,
            request.ManagerPhone,
            request.TotalCapacity,
            request.CapacityUnit,
            request.IsMainWarehouse);

        // Update code via domain method if changed
        if (!string.Equals(warehouse.Code, request.Code, StringComparison.OrdinalIgnoreCase))
        {
            updatedWarehouse.UpdateCode(request.Code);
        }

        // Update warehouse type separately if changed
        if (!string.Equals(warehouse.WarehouseType, request.WarehouseType, StringComparison.OrdinalIgnoreCase))
        {
            updatedWarehouse.UpdateWarehouseType(request.WarehouseType);
        }

        // Handle activation/deactivation separately with proper business rules
        if (warehouse.IsActive != request.IsActive)
        {
            if (request.IsActive)
            {
                updatedWarehouse.Activate();
            }
            else
            {
                if (!warehouse.CanBeDeactivated())
                {
                    throw new WarehouseDeactivationNotAllowedException(warehouse.Id, warehouse.InventoryTransactions.Count);
                }
                updatedWarehouse.Deactivate();
            }
        }
            
        await repository.UpdateAsync(updatedWarehouse, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("warehouse with id : {WarehouseId} updated.", warehouse.Id);
        return new UpdateWarehouseResponse(warehouse.Id);
    }
}
