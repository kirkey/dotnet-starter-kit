namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

public sealed record AddInventoryTransferItemCommand(
    DefaultIdType InventoryTransferId,
    DefaultIdType GroceryItemId,
    int Quantity,
    decimal UnitPrice) : IRequest<AddInventoryTransferItemResponse>;

