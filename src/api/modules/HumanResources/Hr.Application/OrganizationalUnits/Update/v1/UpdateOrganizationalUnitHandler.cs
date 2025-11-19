namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Update.v1;

/// <summary>
/// Handler for updating organizational unit.
/// </summary>
public sealed class UpdateOrganizationalUnitHandler(
    ILogger<UpdateOrganizationalUnitHandler> logger,
    [FromKeyedServices("hr:organizationalunits")] IRepository<OrganizationalUnit> repository)
    : IRequestHandler<UpdateOrganizationalUnitCommand, UpdateOrganizationalUnitResponse>
{
    public async Task<UpdateOrganizationalUnitResponse> Handle(UpdateOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (unit is null)
        {
            throw new OrganizationalUnitNotFoundException(request.Id);
        }

        unit.Update(request.Name, request.ManagerId, request.CostCenter, request.Location);

        await repository.UpdateAsync(unit, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Organizational unit {UnitId} updated successfully", unit.Id);

        return new UpdateOrganizationalUnitResponse(unit.Id);
    }
}

