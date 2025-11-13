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

        return new OrganizationalUnitResponse
        {
            Id = unit.Id,
            CompanyId = unit.CompanyId,
            Code = unit.Code,
            Name = unit.Name,
            Type = unit.Type,
            ParentId = unit.ParentId,
            ParentName = unit.Parent?.Name,
            Level = unit.Level,
            HierarchyPath = unit.HierarchyPath,
            ManagerId = unit.ManagerId,
            CostCenter = unit.CostCenter,
            Location = unit.Location,
            IsActive = unit.IsActive
        };
    }
}

