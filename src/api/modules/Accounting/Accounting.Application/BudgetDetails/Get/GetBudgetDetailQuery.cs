using Accounting.Application.BudgetDetails.Responses;

namespace Accounting.Application.BudgetDetails.Get;

/// <summary>
/// Query to get a budget detail by Id.
/// </summary>
public sealed record GetBudgetDetailQuery(DefaultIdType Id) : IRequest<BudgetDetailResponse>;

