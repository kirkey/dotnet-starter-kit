namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Update.v1;

/// <summary>
/// Command to update an existing inventory transfer item (quantity and unit price).
/// </summary>
public sealed record UpdateInventoryTransferItemCommand(
    DefaultIdType InventoryTransferId,
    DefaultIdType ItemId,
    int Quantity,
    decimal UnitPrice) : IRequest<UpdateInventoryTransferItemResponse>;

