using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Transfers.Create.v1;

public sealed class CreateTransferHandler(
    ILogger<CreateTransferHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<StoreTransfer> repository)
    : IRequestHandler<CreateTransferCommand, CreateTransferResponse>
{
    public async Task<CreateTransferResponse> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        var entity = StoreTransfer.Create(request.TransferNumber, request.TransferDate, request.Notes, request.CreatedByName, request.FromWarehouseId, request.ToStoreId);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("store transfer created {StoreTransferId}", entity.Id);
        return new CreateTransferResponse(entity.Id);
    }
}

