using Store.Domain.Exceptions.Items;
using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

public sealed class AddInventoryTransferItemHandler(
    ILogger<AddInventoryTransferItemHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository,
    [FromKeyedServices("store:items")] IReadRepository<Item> itemReadRepository)
    : IRequestHandler<AddInventoryTransferItemCommand, AddInventoryTransferItemResponse>
{
    public async Task<AddInventoryTransferItemResponse> Handle(AddInventoryTransferItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transfer = await repository.GetByIdAsync(request.InventoryTransferId, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.InventoryTransferId);

        var item = await itemReadRepository.GetByIdAsync(request.ItemId, cancellationToken).ConfigureAwait(false);
        _ = item ?? throw new ItemNotFoundException(request.ItemId);

        transfer.AddItem(request.ItemId, request.Quantity, request.UnitPrice);
        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);

        var itemId = transfer.Items.Last().Id;
        logger.LogInformation("added item {ItemId} to inventory transfer {TransferId}", itemId, transfer.Id);
        return new AddInventoryTransferItemResponse(itemId, transfer.Id);
    }
}

