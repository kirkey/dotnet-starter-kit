using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;

/// <summary>
/// Handler for getting organizational unit by ID.
/// </summary>
public sealed class GetOrganizationalUnitHandler(
    [FromKeyedServices("hr:organizationalunits")] IReadRepository<OrganizationalUnit> repository)
    : IRequestHandler<GetOrganizationalUnitRequest, OrganizationalUnitResponse>
{
    public async Task<OrganizationalUnitResponse> Handle(GetOrganizationalUnitRequest request, CancellationToken cancellationToken)
    {
        var unit = await repository
            .FirstOrDefaultAsync(new OrganizationalUnitByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (unit is null)
        {
            throw new OrganizationalUnitNotFoundException(request.Id);
        }

        return unit;
    }
}

