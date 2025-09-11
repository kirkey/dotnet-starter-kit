using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Starter.WebApi.Warehouse.Domain;

namespace FSH.Starter.WebApi.Warehouse.Features.Stores.Update.v1;

public sealed class UpdateStoreHandler(
    ILogger<UpdateStoreHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Store> repository)
    : IRequestHandler<UpdateStoreCommand, UpdateStoreResponse>
{
    public async Task<UpdateStoreResponse> Handle(UpdateStoreCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new WarehouseNotFoundException(request.Id);
        var updated = entity.Update(request.Name, request.Code, request.Address, request.Phone, request.Manager, request.IsActive, request.CompanyId);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("store updated {StoreId}", updated.Id);
        return new UpdateStoreResponse(updated.Id);
    }
}

