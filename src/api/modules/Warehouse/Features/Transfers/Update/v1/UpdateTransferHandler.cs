using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Transfers.Update.v1;

public sealed class UpdateTransferHandler(
    ILogger<UpdateTransferHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<StoreTransfer> repository)
    : IRequestHandler<UpdateTransferCommand, UpdateTransferResponse>
{
    public async Task<UpdateTransferResponse> Handle(UpdateTransferCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new StoreTransferNotFoundException(request.Id);
        entity.Update(request.TransferNumber, request.TransferDate, request.ReceivedDate, request.Status, request.Notes, request.ReceivedBy);
        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("store transfer updated {StoreTransferId}", entity.Id);
        return new UpdateTransferResponse(entity.Id);
    }
}

