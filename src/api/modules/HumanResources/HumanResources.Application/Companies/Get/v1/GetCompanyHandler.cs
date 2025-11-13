using FSH.Starter.WebApi.HumanResources.Application.Companies.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;

/// <summary>
/// Handler for getting company by ID.
/// </summary>
public sealed class GetCompanyHandler(
    [FromKeyedServices("hr:companies")] IReadRepository<Company> repository)
    : IRequestHandler<GetCompanyRequest, CompanyResponse>
{
    public async Task<CompanyResponse> Handle(GetCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await repository
            .FirstOrDefaultAsync(new CompanyByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (company is null)
        {
            throw new CompanyNotFoundException(request.Id);
        }

        return new CompanyResponse
        {
            Id = company.Id,
            Code = company.CompanyCode,
            Name = company.Name,
            TIN = company.Tin,
            Address = company.Address,
            ZipCode = company.ZipCode,
            Phone = company.Phone,
            Email = company.Email,
            Website = company.Website,
            LogoUrl = company.LogoUrl,
            IsActive = company.IsActive
        };
    }
}

