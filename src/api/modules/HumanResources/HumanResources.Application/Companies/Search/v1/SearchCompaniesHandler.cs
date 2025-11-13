using FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Companies.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Search.v1;

/// <summary>
/// Handler for searching companies.
/// </summary>
public sealed class SearchCompaniesHandler(
    [FromKeyedServices("hr:companies")] IReadRepository<Company> repository)
    : IRequestHandler<SearchCompaniesRequest, PagedList<CompanyResponse>>
{
    public async Task<PagedList<CompanyResponse>> Handle(SearchCompaniesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCompaniesSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<CompanyResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

