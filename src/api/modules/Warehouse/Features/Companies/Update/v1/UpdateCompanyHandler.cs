using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using FSH.Starter.WebApi.Warehouse.Domain;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Update.v1;

public sealed class UpdateCompanyHandler(
    ILogger<UpdateCompanyHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Company> repository)
    : IRequestHandler<UpdateCompanyCommand, UpdateCompanyResponse>
{
    public async Task<UpdateCompanyResponse> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new WarehouseNotFoundException(request.Id);
        var updated = entity.Update(request.Name, request.Address, request.Phone, request.Email, request.TaxId);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("company updated {CompanyId}", updated.Id);
        return new UpdateCompanyResponse(updated.Id);
    }
}

