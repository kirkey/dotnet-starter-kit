using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Search.v1;

/// <summary>
/// Handler for searching organizational units.
/// </summary>
public sealed class SearchOrganizationalUnitsHandler(
    [FromKeyedServices("hr:organizationalunits")] IReadRepository<OrganizationalUnit> repository)
    : IRequestHandler<SearchOrganizationalUnitsRequest, PagedList<OrganizationalUnitResponse>>
{
    public async Task<PagedList<OrganizationalUnitResponse>> Handle(SearchOrganizationalUnitsRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchOrganizationalUnitsSpec(request);

        var items = await repository
            .ListAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        var totalCount = await repository
            .CountAsync(spec, cancellationToken)
            .ConfigureAwait(false);

        return new PagedList<OrganizationalUnitResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

