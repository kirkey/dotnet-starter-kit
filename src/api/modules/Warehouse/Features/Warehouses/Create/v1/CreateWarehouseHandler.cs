using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Create.v1;

public sealed class CreateWarehouseHandler(
    ILogger<CreateWarehouseHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Warehouse.Domain.Warehouse> repository)
    : IRequestHandler<CreateWarehouseCommand, CreateWarehouseResponse>
{
    public async Task<CreateWarehouseResponse> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = Warehouse.Domain.Warehouse.Create(request.Name, request.Code, request.Address, request.Phone, request.Manager, request.IsActive, request.CompanyId);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("warehouse created {WarehouseId}", entity.Id);
        return new CreateWarehouseResponse(entity.Id);
    }
}

