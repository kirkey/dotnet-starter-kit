


using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

public sealed class UpdateInventoryTransferHandler(
    ILogger<UpdateInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> warehouseRepository)
    : IRequestHandler<UpdateInventoryTransferCommand, UpdateInventoryTransferResponse>
{
    public async Task<UpdateInventoryTransferResponse> Handle(UpdateInventoryTransferCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var inventoryTransfer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = inventoryTransfer ?? throw new InventoryTransferNotFoundException(request.Id);
        
        // Verify warehouses exist if changed
        if (inventoryTransfer.FromWarehouseId != request.FromWarehouseId)
        {
            var fromWarehouse = await warehouseRepository.GetByIdAsync(request.FromWarehouseId, cancellationToken).ConfigureAwait(false);
            _ = fromWarehouse ?? throw new WarehouseNotFoundException(request.FromWarehouseId);
        }
        
        if (inventoryTransfer.ToWarehouseId != request.ToWarehouseId)
        {
            var toWarehouse = await warehouseRepository.GetByIdAsync(request.ToWarehouseId, cancellationToken).ConfigureAwait(false);
            _ = toWarehouse ?? throw new WarehouseNotFoundException(request.ToWarehouseId);
        }
        
        var updatedInventoryTransfer = inventoryTransfer.Update(
            request.Name,
            request.Description,
            request.TransferNumber,
            request.FromWarehouseId,
            request.ToWarehouseId,
            request.TransferDate,
            request.Status,
            request.Notes,
            request.Reason);
            
        await repository.UpdateAsync(updatedInventoryTransfer, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("inventory transfer with id : {InventoryTransferId} updated.", inventoryTransfer.Id);
        return new UpdateInventoryTransferResponse(inventoryTransfer.Id);
    }
}
