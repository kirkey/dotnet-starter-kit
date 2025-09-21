namespace Accounting.Application.Budgets.BudgetDetails.Commands;

/// <summary>
/// Query to get all budget details (non-paginated) for a budget.
/// </summary>
public sealed record BudgetGetDetailsCommand(DefaultIdType BudgetId) : IRequest<List<Responses.BudgetDetailResponse>>;
