using FSH.Framework.Core.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Create.v1;

/// <summary>
/// Handler for creating a new company.
/// </summary>
public sealed class CreateCompanyHandler(
    ILogger<CreateCompanyHandler> logger,
    [FromKeyedServices("hr:companies")] IRepository<Company> repository)
    : IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
{
    public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create company using domain factory method
        var company = Company.Create(
            request.CompanyCode,
            request.Name,
            request.Tin);

        // Persist to database
        await repository.AddAsync(company, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Company created with ID {CompanyId} and code {CompanyCode}", company.Id, company.CompanyCode);

        return new CreateCompanyResponse(company.Id);
    }
}

