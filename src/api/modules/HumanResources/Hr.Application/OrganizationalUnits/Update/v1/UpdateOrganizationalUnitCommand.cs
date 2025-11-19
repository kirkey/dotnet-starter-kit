namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Update.v1;

/// <summary>
/// Command to update organizational unit.
/// </summary>
public sealed record UpdateOrganizationalUnitCommand(
    DefaultIdType Id,
    [property: DefaultValue("Human Resources Department")] string Name,
    [property: DefaultValue(null)] DefaultIdType? ManagerId = null,
    [property: DefaultValue(null)] string? CostCenter = null,
    [property: DefaultValue(null)] string? Location = null) : IRequest<UpdateOrganizationalUnitResponse>;

