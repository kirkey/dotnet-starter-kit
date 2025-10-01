namespace FSH.Starter.WebApi.Store.Application.Warehouses.AssignManager.v1;

public sealed class AssignWarehouseManagerHandler(
    ILogger<AssignWarehouseManagerHandler> logger,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> repository)
    : IRequestHandler<AssignWarehouseManagerCommand, AssignWarehouseManagerResponse>
{
    public async Task<AssignWarehouseManagerResponse> Handle(AssignWarehouseManagerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var warehouse = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = warehouse ?? throw new WarehouseNotFoundException(request.Id);

        // Use domain method to assign manager which performs validation and queues event
        warehouse.AssignManager(request.ManagerName, request.ManagerEmail, request.ManagerPhone);

        await repository.UpdateAsync(warehouse, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Assigned manager to warehouse {WarehouseId}", warehouse.Id);

        return new AssignWarehouseManagerResponse(warehouse.Id);
    }
}

