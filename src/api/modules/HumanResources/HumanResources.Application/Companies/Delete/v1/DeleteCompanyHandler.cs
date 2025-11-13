namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Delete.v1;

/// <summary>
/// Handler for deleting company.
/// </summary>
public sealed class DeleteCompanyHandler(
    ILogger<DeleteCompanyHandler> logger,
    [FromKeyedServices("hr:companies")] IRepository<Company> repository)
    : IRequestHandler<DeleteCompanyCommand, DeleteCompanyResponse>
{
    public async Task<DeleteCompanyResponse> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (company is null)
        {
            throw new CompanyNotFoundException(request.Id);
        }

        await repository.DeleteAsync(company, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Company {CompanyId} deleted successfully", company.Id);

        return new DeleteCompanyResponse(company.Id);
    }
}

