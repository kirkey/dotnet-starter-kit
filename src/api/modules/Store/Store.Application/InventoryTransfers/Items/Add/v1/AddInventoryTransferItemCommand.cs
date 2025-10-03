namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

public sealed record AddInventoryTransferItemCommand(
    DefaultIdType InventoryTransferId,
    DefaultIdType ItemId,
    int Quantity,
    decimal UnitPrice) : IRequest<AddInventoryTransferItemResponse>;

