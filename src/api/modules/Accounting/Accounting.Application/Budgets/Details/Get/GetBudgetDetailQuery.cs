using Accounting.Application.Budgets.Details.Responses;

namespace Accounting.Application.Budgets.Details.Get;

/// <summary>
/// Query to get a budget detail by Id.
/// </summary>
public sealed record GetBudgetDetailQuery(DefaultIdType Id) : IRequest<BudgetDetailResponse>;

