using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Transfers.Create.v1;

public sealed record CreateTransferCommand(
    [property: DefaultValue("TR-0001")] string TransferNumber,
    DateTime TransferDate,
    string? Notes,
    string CreatedByName,
    DefaultIdType FromWarehouseId,
    DefaultIdType ToStoreId) : IRequest<CreateTransferResponse>;

