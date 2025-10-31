namespace Accounting.Application.CostCenters.Queries;

/// <summary>
/// Cost center data transfer object for list views.
/// </summary>
public record CostCenterDto
{
    public DefaultIdType Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string CostCenterType { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public decimal BudgetAmount { get; init; }
    public decimal ActualAmount { get; init; }
    public decimal Variance { get; init; }
}

/// <summary>
/// Cost center data transfer object for detail view with all properties.
/// </summary>
public record CostCenterDetailsDto : CostCenterDto
{
    public DefaultIdType? ParentCostCenterId { get; init; }
    public DefaultIdType? ManagerId { get; init; }
    public string? ManagerName { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
}

