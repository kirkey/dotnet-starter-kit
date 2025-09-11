using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Transfers.Update.v1;

public sealed record UpdateTransferCommand(
    DefaultIdType Id,
    string? TransferNumber,
    DateTime? TransferDate,
    DateTime? ReceivedDate,
    TransferStatus? Status,
    string? Notes,
    string? ReceivedBy) : IRequest<UpdateTransferResponse>;

