namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Approve.v1;

public sealed record ApproveInventoryTransferCommand(DefaultIdType Id, string ApprovedBy) : IRequest<ApproveInventoryTransferResponse>;

