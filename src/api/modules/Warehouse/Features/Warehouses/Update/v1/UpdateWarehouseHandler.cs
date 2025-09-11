using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Update.v1;

public sealed class UpdateWarehouseHandler(
    ILogger<UpdateWarehouseHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Warehouse.Domain.Warehouse> repository)
    : IRequestHandler<UpdateWarehouseCommand, UpdateWarehouseResponse>
{
    public async Task<UpdateWarehouseResponse> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new WarehouseNotFoundException(request.Id);
        var updated = entity.Update(request.Name, request.Code, request.Address, request.Phone, request.Manager, request.IsActive, request.CompanyId);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("warehouse updated {WarehouseId}", updated.Id);
        return new UpdateWarehouseResponse(updated.Id);
    }
}

