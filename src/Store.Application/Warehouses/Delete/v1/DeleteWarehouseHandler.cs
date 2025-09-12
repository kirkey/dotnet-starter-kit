


namespace FSH.Starter.WebApi.Store.Application.Warehouses.Delete.v1;

public sealed class DeleteWarehouseHandler(
    ILogger<DeleteWarehouseHandler> logger,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> repository)
    : IRequestHandler<DeleteWarehouseCommand>
{
    public async Task Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var warehouse = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = warehouse ?? throw new WarehouseNotFoundException(request.Id);
        
        await repository.DeleteAsync(warehouse, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("warehouse deleted {WarehouseId}", warehouse.Id);
    }
}
