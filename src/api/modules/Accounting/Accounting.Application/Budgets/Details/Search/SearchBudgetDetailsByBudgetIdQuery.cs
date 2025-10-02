using Accounting.Application.Budgets.Details.Responses;

namespace Accounting.Application.Budgets.Details.Search;

/// <summary>
/// Query to fetch all details for a given budget.
/// </summary>
public sealed record SearchBudgetDetailsByBudgetIdQuery(DefaultIdType BudgetId) : IRequest<List<BudgetDetailResponse>>;

