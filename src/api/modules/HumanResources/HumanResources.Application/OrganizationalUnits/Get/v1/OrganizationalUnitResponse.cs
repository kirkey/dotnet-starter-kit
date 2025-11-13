namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Get.v1;

/// <summary>
/// Response for organizational unit details.
/// </summary>
public sealed record OrganizationalUnitResponse
{
    public DefaultIdType Id { get; init; }
    public DefaultIdType CompanyId { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
    public OrganizationalUnitType Type { get; init; }
    public DefaultIdType? ParentId { get; init; }
    public string? ParentName { get; init; }
    public int Level { get; init; }
    public string? HierarchyPath { get; init; }
    public DefaultIdType? ManagerId { get; init; }
    public string? ManagerName { get; init; }
    public string? CostCenter { get; init; }
    public string? Location { get; init; }
    public bool IsActive { get; init; }
}

