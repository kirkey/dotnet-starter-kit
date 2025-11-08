namespace Accounting.Application.CostCenters.UpdateBudget.v1;

public sealed record UpdateBudgetCommand(DefaultIdType Id, decimal BudgetAmount) : IRequest<DefaultIdType>;

