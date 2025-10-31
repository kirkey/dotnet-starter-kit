namespace Accounting.Application.CostCenters.Create.v1;

/// <summary>
/// Command to create a new cost center.
/// </summary>
public record CostCenterCreateCommand(
    string Code,
    string Name,
    string CostCenterType,
    DefaultIdType? ParentCostCenterId = null,
    DefaultIdType? ManagerId = null,
    string? ManagerName = null,
    decimal BudgetAmount = 0,
    string? Description = null,
    string? Notes = null
) : IRequest<CostCenterCreateResponse>;

