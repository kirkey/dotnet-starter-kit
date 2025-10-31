namespace Accounting.Application.CostCenters.Responses;

/// <summary>
/// Response containing cost center details.
/// </summary>
public record CostCenterResponse
{
    /// <summary>
    /// Cost center unique identifier.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Unique cost center code (e.g., "DEPT-001", "DIV-SALES").
    /// </summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Cost center name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Cost center type: Department, Division, BusinessUnit, Project, Location.
    /// </summary>
    public string CostCenterType { get; init; } = string.Empty;

    /// <summary>
    /// Whether the cost center is active and available for use.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Reference to parent cost center for hierarchical rollup.
    /// </summary>
    public DefaultIdType? ParentCostCenterId { get; init; }

    /// <summary>
    /// Optional manager/supervisor responsible for this cost center.
    /// </summary>
    public DefaultIdType? ManagerId { get; init; }

    /// <summary>
    /// Manager name for display purposes.
    /// </summary>
    public string? ManagerName { get; init; }

    /// <summary>
    /// Optional budget allocated to this cost center.
    /// </summary>
    public decimal BudgetAmount { get; init; }

    /// <summary>
    /// Actual expenses/costs incurred (updated from transactions).
    /// </summary>
    public decimal ActualAmount { get; init; }

    /// <summary>
    /// Optional location or site associated with this cost center.
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Optional start date when cost center becomes active.
    /// </summary>
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Optional end date when cost center is closed.
    /// </summary>
    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Optional description or purpose of the cost center.
    /// </summary>
    public string? Description { get; init; }
}

