namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Create.v1;

/// <summary>
/// Command to create a new organizational unit.
/// </summary>
public sealed record CreateOrganizationalUnitCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType CompanyId,
    [property: DefaultValue("HR-001")] string Code,
    [property: DefaultValue("Human Resources Department")] string Name,
    [property: DefaultValue(OrganizationalUnitType.Department)] OrganizationalUnitType Type,
    [property: DefaultValue(null)] DefaultIdType? ParentId = null,
    [property: DefaultValue(null)] DefaultIdType? ManagerId = null,
    [property: DefaultValue(null)] string? CostCenter = null,
    [property: DefaultValue(null)] string? Location = null) : IRequest<CreateOrganizationalUnitResponse>;

