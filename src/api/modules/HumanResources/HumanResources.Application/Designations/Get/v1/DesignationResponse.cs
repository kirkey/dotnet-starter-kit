namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;

/// <summary>
/// Response for designation details.
/// </summary>
public sealed record DesignationResponse
{
    public DefaultIdType Id { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public DefaultIdType OrganizationalUnitId { get; init; }
    public string? OrganizationalUnitName { get; init; }
    public string? Description { get; init; }
    public decimal? MinSalary { get; init; }
    public decimal? MaxSalary { get; init; }
    public bool IsActive { get; init; }
}

