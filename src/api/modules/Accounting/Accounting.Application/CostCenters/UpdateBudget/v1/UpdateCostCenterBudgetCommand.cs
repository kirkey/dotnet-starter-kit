namespace Accounting.Application.CostCenters.UpdateBudget.v1;

/// <summary>
/// Command to update the budget amount for a cost center.
/// </summary>
public sealed record UpdateCostCenterBudgetCommand(DefaultIdType Id, decimal BudgetAmount) : IRequest<DefaultIdType>;

