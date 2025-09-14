using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Remove.v1;

public sealed class RemoveInventoryTransferItemHandler(
    ILogger<RemoveInventoryTransferItemHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository)
    : IRequestHandler<RemoveInventoryTransferItemCommand>
{
    public async Task Handle(RemoveInventoryTransferItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transfer = await repository.GetByIdAsync(request.InventoryTransferId, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.InventoryTransferId);

        var item = transfer.Items.FirstOrDefault(i => i.Id == request.ItemId);
        _ = item ?? throw new InventoryTransferItemNotFoundException(request.ItemId);

        transfer.RemoveItem(request.ItemId);
        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("removed item {ItemId} from inventory transfer {TransferId}", request.ItemId, transfer.Id);
    }
}

