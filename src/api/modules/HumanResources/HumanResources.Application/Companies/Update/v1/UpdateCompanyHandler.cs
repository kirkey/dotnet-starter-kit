namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Update.v1;

/// <summary>
/// Handler for updating company.
/// </summary>
public sealed class UpdateCompanyHandler(
    ILogger<UpdateCompanyHandler> logger,
    [FromKeyedServices("hr:companies")] IRepository<Company> repository)
    : IRequestHandler<UpdateCompanyCommand, UpdateCompanyResponse>
{
    public async Task<UpdateCompanyResponse> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (company is null)
        {
            throw new CompanyNotFoundException(request.Id);
        }

        company.Update(request.Name, request.TIN);
        company.UpdateAddress(request.Address, request.ZipCode);
        company.UpdateContact(request.Phone, request.Email, request.Website);

        await repository.UpdateAsync(company, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Company {CompanyId} updated successfully", company.Id);

        return new UpdateCompanyResponse(company.Id);
    }
}

