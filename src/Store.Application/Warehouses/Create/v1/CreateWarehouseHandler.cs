

namespace FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;

public sealed class CreateWarehouseHandler(
    ILogger<CreateWarehouseHandler> logger,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> repository)
    : IRequestHandler<CreateWarehouseCommand, CreateWarehouseResponse>
{
    public async Task<CreateWarehouseResponse> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var warehouse = Warehouse.Create(
            request.Name,
            request.Description,
            request.Code,
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
            request.IsActive,
            request.IsMainWarehouse);
            
        await repository.AddAsync(warehouse, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("warehouse created {WarehouseId}", warehouse.Id);
        return new CreateWarehouseResponse(warehouse.Id);
    }
}
