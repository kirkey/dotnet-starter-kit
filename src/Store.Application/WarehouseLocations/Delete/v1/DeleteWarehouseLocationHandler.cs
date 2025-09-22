namespace FSH.Starter.WebApi.Store.Application.WarehouseLocations.Delete.v1;

public sealed class DeleteWarehouseLocationHandler(
    ILogger<DeleteWarehouseLocationHandler> logger,
    [FromKeyedServices("store:warehouse-locations")] IRepository<WarehouseLocation> repository)
    : IRequestHandler<DeleteWarehouseLocationCommand>
{
    public async Task Handle(DeleteWarehouseLocationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var warehouseLocation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = warehouseLocation ?? throw new WarehouseLocationNotFoundException(request.Id);
        
        // Enforce business rule: cannot delete location with used capacity or stored items
        if (!warehouseLocation.CanBeDeleted())
        {
            throw new WarehouseLocationDeletionNotAllowedException(warehouseLocation.Id, warehouseLocation.UsedCapacity);
        }
        
        await repository.DeleteAsync(warehouseLocation, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("warehouse location deleted {WarehouseLocationId}", warehouseLocation.Id);
    }
}
