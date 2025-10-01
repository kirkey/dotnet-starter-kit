namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;

public sealed class CreateInventoryTransferHandler(
    ILogger<CreateInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> warehouseRepository)
    : IRequestHandler<CreateInventoryTransferCommand, CreateInventoryTransferResponse>
{
    public async Task<CreateInventoryTransferResponse> Handle(CreateInventoryTransferCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        // Verify both warehouses exist
        var fromWarehouse = await warehouseRepository.GetByIdAsync(request.FromWarehouseId, cancellationToken).ConfigureAwait(false);
        _ = fromWarehouse ?? throw new WarehouseNotFoundException(request.FromWarehouseId);
        
        var toWarehouse = await warehouseRepository.GetByIdAsync(request.ToWarehouseId, cancellationToken).ConfigureAwait(false);
        _ = toWarehouse ?? throw new WarehouseNotFoundException(request.ToWarehouseId);
        
        var inventoryTransfer = InventoryTransfer.Create(
            null,
            null,
            request.TransferNumber,
            request.FromWarehouseId,
            request.ToWarehouseId,
            request.FromLocationId,
            request.ToLocationId,
            request.TransferDate,
            request.ExpectedArrivalDate,
            request.TransferType,
            request.Priority,
            request.TransportMethod,
            request.Notes,
            request.RequestedBy);
            
        await repository.AddAsync(inventoryTransfer, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("inventory transfer created {InventoryTransferId}", inventoryTransfer.Id);
        return new CreateInventoryTransferResponse(inventoryTransfer.Id);
    }
}
