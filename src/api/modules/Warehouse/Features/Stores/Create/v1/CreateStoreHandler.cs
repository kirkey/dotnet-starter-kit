using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Stores.Create.v1;

public sealed class CreateStoreHandler(
    ILogger<CreateStoreHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Store> repository)
    : IRequestHandler<CreateStoreCommand, CreateStoreResponse>
{
    public async Task<CreateStoreResponse> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = Store.Create(request.Name, request.Code, request.Address, request.Phone, request.Manager, request.IsActive, request.CompanyId);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("store created {StoreId}", entity.Id);
        return new CreateStoreResponse(entity.Id);
    }
}

