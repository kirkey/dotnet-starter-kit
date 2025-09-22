namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Complete.v1;

public sealed record CompleteInventoryTransferCommand(DefaultIdType Id, DateTime ActualArrival) : IRequest<CompleteInventoryTransferResponse>;

