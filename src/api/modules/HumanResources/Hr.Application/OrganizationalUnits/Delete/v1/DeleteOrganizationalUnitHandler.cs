using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Delete.v1;

/// <summary>
/// Handler for deleting organizational unit.
/// </summary>
public sealed class DeleteOrganizationalUnitHandler(
    ILogger<DeleteOrganizationalUnitHandler> logger,
    [FromKeyedServices("hr:organizationalunits")] IRepository<OrganizationalUnit> repository,
    [FromKeyedServices("hr:organizationalunits")] IReadRepository<OrganizationalUnit> readRepository)
    : IRequestHandler<DeleteOrganizationalUnitCommand, DeleteOrganizationalUnitResponse>
{
    public async Task<DeleteOrganizationalUnitResponse> Handle(DeleteOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (unit is null)
        {
            throw new OrganizationalUnitNotFoundException(request.Id);
        }

        // Check if unit has children
        var hasChildren = await readRepository
            .AnyAsync(new OrganizationalUnitsByParentSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (hasChildren)
        {
            throw new InvalidOrganizationalHierarchyException("Cannot delete organizational unit with child units. Delete or reassign children first.");
        }

        await repository.DeleteAsync(unit, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Organizational unit {UnitId} deleted successfully", unit.Id);

        return new DeleteOrganizationalUnitResponse(unit.Id);
    }
}

