using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Create.v1;

public sealed class CreateSupplierHandler(
    ILogger<CreateSupplierHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Supplier> repository)
    : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    public async Task<CreateSupplierResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var entity = Supplier.Create(request.Name, request.Code, request.ContactPerson, request.Address, request.Phone, request.Email, request.TaxId, request.PaymentTermsDays, request.IsActive);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("supplier created {SupplierId}", entity.Id);
        return new CreateSupplierResponse(entity.Id);
    }
}
