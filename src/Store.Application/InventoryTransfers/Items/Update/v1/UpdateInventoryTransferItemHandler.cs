using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Update.v1;

public sealed class UpdateInventoryTransferItemHandler(
    ILogger<UpdateInventoryTransferItemHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<UpdateInventoryTransferItemCommand, UpdateInventoryTransferItemResponse>
{
    public async Task<UpdateInventoryTransferItemResponse> Handle(UpdateInventoryTransferItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transfer = await repository.GetByIdAsync(request.InventoryTransferId, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.InventoryTransferId);

        // Delegate state and validation to domain
        transfer.UpdateItem(request.ItemId, request.Quantity, request.UnitPrice);

        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated item {ItemId} on inventory transfer {TransferId}", request.ItemId, transfer.Id);
        return new UpdateInventoryTransferItemResponse(request.ItemId, transfer.Id);
    }
}
