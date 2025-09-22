using Accounting.Application.BudgetDetails.Responses;

namespace Accounting.Application.BudgetDetails.Search;

/// <summary>
/// Query to fetch all details for a given budget.
/// </summary>
public sealed record SearchBudgetDetailsByBudgetIdQuery(DefaultIdType BudgetId) : IRequest<List<BudgetDetailResponse>>;

