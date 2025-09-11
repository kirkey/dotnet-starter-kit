using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Update.v1;

public sealed class UpdateSupplierHandler(
    ILogger<UpdateSupplierHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Supplier> repository)
    : IRequestHandler<UpdateSupplierCommand, UpdateSupplierResponse>
{
    public async Task<UpdateSupplierResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new SupplierNotFoundException(request.Id);
        entity.Update(request.Name, request.Code, request.ContactPerson, request.Address, request.Phone, request.Email, request.TaxId, request.PaymentTermsDays, request.IsActive);
        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("supplier updated {SupplierId}", entity.Id);
        return new UpdateSupplierResponse(entity.Id);
    }
}

