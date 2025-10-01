namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Remove.v1;

public sealed record RemoveInventoryTransferItemCommand(DefaultIdType InventoryTransferId, DefaultIdType ItemId) : IRequest;

