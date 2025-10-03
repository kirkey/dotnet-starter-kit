using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Delete.v1;

public sealed class DeleteInventoryTransferHandler(
    ILogger<DeleteInventoryTransferHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<DeleteInventoryTransferCommand>
{
    public async Task Handle(DeleteInventoryTransferCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var inventoryTransfer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = inventoryTransfer ?? throw new InventoryTransferNotFoundException(request.Id);
        
        await repository.DeleteAsync(inventoryTransfer, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("inventory transfer deleted {InventoryTransferId}", inventoryTransfer.Id);
    }
}
