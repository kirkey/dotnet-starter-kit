namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Cancel.v1;

public sealed record CancelInventoryTransferCommand(DefaultIdType Id, string? Reason) : IRequest<CancelInventoryTransferResponse>;

