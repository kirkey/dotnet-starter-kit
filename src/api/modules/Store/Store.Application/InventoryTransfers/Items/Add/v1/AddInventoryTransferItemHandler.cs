using Store.Domain.Exceptions.GroceryItem;
using Store.Domain.Exceptions.InventoryTransfer;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

public sealed class AddInventoryTransferItemHandler(
    ILogger<AddInventoryTransferItemHandler> logger,
    [FromKeyedServices("store:inventory-transfers")] IRepository<InventoryTransfer> repository,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> groceryReadRepository)
    : IRequestHandler<AddInventoryTransferItemCommand, AddInventoryTransferItemResponse>
{
    public async Task<AddInventoryTransferItemResponse> Handle(AddInventoryTransferItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transfer = await repository.GetByIdAsync(request.InventoryTransferId, cancellationToken).ConfigureAwait(false);
        _ = transfer ?? throw new InventoryTransferNotFoundException(request.InventoryTransferId);

        var gi = await groceryReadRepository.GetByIdAsync(request.GroceryItemId, cancellationToken).ConfigureAwait(false);
        _ = gi ?? throw new GroceryItemNotFoundException(request.GroceryItemId);

        transfer.AddItem(request.GroceryItemId, request.Quantity, request.UnitPrice);
        await repository.UpdateAsync(transfer, cancellationToken).ConfigureAwait(false);

        var itemId = transfer.Items.Last().Id;
        logger.LogInformation("added item {ItemId} to inventory transfer {TransferId}", itemId, transfer.Id);
        return new AddInventoryTransferItemResponse(itemId, transfer.Id);
    }
}

