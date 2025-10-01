namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.MarkInTransit.v1;

/// <summary>
/// Command to mark an inventory transfer as InTransit.
/// </summary>
public sealed record MarkInTransitInventoryTransferCommand(DefaultIdType Id) : IRequest<MarkInTransitInventoryTransferResponse>;

