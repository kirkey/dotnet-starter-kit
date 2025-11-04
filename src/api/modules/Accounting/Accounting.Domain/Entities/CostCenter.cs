using Accounting.Domain.Events.CostCenter;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a cost center (department, division, or business unit) for expense allocation and internal reporting.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track expenses and revenues by department or division.
/// - Enable internal financial reporting and profitability analysis.
/// - Support departmental budgeting and variance analysis.
/// - Facilitate cost allocation and inter-departmental charging.
/// - Provide organizational hierarchy for management reporting.
/// - Enable project costing and job tracking by department.
/// - Support transfer pricing and cost recovery mechanisms.
/// 
/// Default values:
/// - CostCenterType: Department (most common organizational unit)
/// - IsActive: true (cost center is active and available)
/// - ParentCostCenterId: null (no parent, top-level cost center)
/// - BudgetAmount: 0.00 (no budget assigned yet)
/// - ActualAmount: 0.00 (no transactions recorded yet)
/// - ManagerId: null (no manager assigned)
/// 
/// Business rules:
/// - Cost center code must be unique
/// - Cannot delete cost center with transactions
/// - Can have parent-child hierarchy for rollup reporting
/// - Budget amounts are optional but recommended for control
/// - Manager assignment enables approval workflows
/// - Inactive cost centers cannot receive new transactions
/// - Cost center can be associated with multiple accounts
/// </remarks>
public class CostCenter : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique cost center code (e.g., "DEPT-001", "DIV-SALES").
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    // Name property inherited from AuditableEntity base class

    /// <summary>
    /// Cost center type: Department, Division, BusinessUnit, Project, Location.
    /// </summary>
    public CostCenterType CostCenterType { get; private set; }

    /// <summary>
    /// Whether the cost center is active and available for use.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Reference to parent cost center for hierarchical rollup.
    /// </summary>
    public DefaultIdType? ParentCostCenterId { get; private set; }

    /// <summary>
    /// Optional manager/supervisor responsible for this cost center.
    /// </summary>
    public DefaultIdType? ManagerId { get; private set; }

    /// <summary>
    /// Manager name for display purposes.
    /// </summary>
    public string? ManagerName { get; private set; }

    /// <summary>
    /// Optional budget allocated to this cost center.
    /// </summary>
    public decimal BudgetAmount { get; private set; }

    /// <summary>
    /// Actual expenses/costs incurred (updated from transactions).
    /// </summary>
    public decimal ActualAmount { get; private set; }

    /// <summary>
    /// Optional location or site associated with this cost center.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Optional start date when cost center becomes active.
    /// </summary>
    public DateTime? StartDate { get; private set; }

    /// <summary>
    /// Optional end date when cost center is closed.
    /// </summary>
    public DateTime? EndDate { get; private set; }

    private CostCenter(
        string code,
        string name,
        CostCenterType costCenterType,
        DefaultIdType? parentCostCenterId = null,
        DefaultIdType? managerId = null,
        string? managerName = null,
        decimal budgetAmount = 0,
        string? location = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? description = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Cost center code is required", nameof(code));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Cost center name is required", nameof(name));

        if (budgetAmount < 0)
            throw new ArgumentException("Budget amount cannot be negative", nameof(budgetAmount));

        if (endDate.HasValue && startDate.HasValue && endDate.Value < startDate.Value)
            throw new ArgumentException("End date cannot be before start date", nameof(endDate));

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        CostCenterType = costCenterType;
        IsActive = true;
        ParentCostCenterId = parentCostCenterId;
        ManagerId = managerId;
        ManagerName = managerName?.Trim();
        BudgetAmount = budgetAmount;
        ActualAmount = 0;
        Location = location?.Trim();
        StartDate = startDate;
        EndDate = endDate;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new CostCenterCreated(Id, Code, Name, CostCenterType));
    }

    /// <summary>
    /// Create a new cost center.
    /// </summary>
    public static CostCenter Create(
        string code,
        string name,
        CostCenterType costCenterType,
        DefaultIdType? parentCostCenterId = null,
        DefaultIdType? managerId = null,
        string? managerName = null,
        decimal budgetAmount = 0,
        string? location = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? description = null,
        string? notes = null)
    {
        return new CostCenter(code, name, costCenterType, parentCostCenterId, managerId,
            managerName, budgetAmount, location, startDate, endDate, description, notes);
    }

    /// <summary>
    /// Update cost center details.
    /// </summary>
    public void Update(
        string? name = null,
        DefaultIdType? managerId = null,
        string? managerName = null,
        string? location = null,
        DateTime? endDate = null,
        string? description = null,
        string? notes = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();

        if (managerId.HasValue)
        {
            ManagerId = managerId;
            ManagerName = managerName?.Trim();
        }

        Location = location?.Trim();
        EndDate = endDate;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new CostCenterUpdated(Id, Name));
    }

    /// <summary>
    /// Update budget allocation for the cost center.
    /// </summary>
    public void UpdateBudget(decimal newBudgetAmount)
    {
        if (newBudgetAmount < 0)
            throw new ArgumentException("Budget amount cannot be negative", nameof(newBudgetAmount));

        BudgetAmount = newBudgetAmount;
        QueueDomainEvent(new CostCenterBudgetUpdated(Id, BudgetAmount));
    }

    /// <summary>
    /// Record actual expenses/costs for this cost center.
    /// </summary>
    public void RecordActual(decimal amount)
    {
        ActualAmount += amount;
        QueueDomainEvent(new CostCenterActualRecorded(Id, amount, ActualAmount));
    }

    /// <summary>
    /// Activate the cost center.
    /// </summary>
    public void Activate()
    {
        if (IsActive)
            throw new CostCenterAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new CostCenterActivated(Id));
    }

    /// <summary>
    /// Deactivate the cost center.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            throw new CostCenterAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new CostCenterDeactivated(Id));
    }

    /// <summary>
    /// Get budget variance (actual vs budget).
    /// </summary>
    public decimal GetVariance()
    {
        return BudgetAmount - ActualAmount;
    }

    /// <summary>
    /// Get budget utilization percentage.
    /// </summary>
    public decimal GetUtilizationPercentage()
    {
        if (BudgetAmount == 0) return 0;
        return Math.Round((ActualAmount / BudgetAmount) * 100, 2);
    }
}

/// <summary>
/// Cost center type classifications.
/// </summary>
public enum CostCenterType
{
    Department,
    Division,
    BusinessUnit,
    Project,
    Location,
    Other
}
