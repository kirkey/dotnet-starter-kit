using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Companies.Create.v1;

public sealed class CreateCompanyHandler(
    ILogger<CreateCompanyHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Company> repository)
    : IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
{
    public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = Company.Create(request.Name, request.Address, request.Phone, request.Email, request.TaxId);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("company created {CompanyId}", entity.Id);
        return new CreateCompanyResponse(entity.Id);
    }
}
