using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.Get;

/// <summary>
/// Query to retrieve a single Budget by identifier.
/// </summary>
/// <param name="Id">Budget identifier.</param>
public sealed record GetBudgetQuery(DefaultIdType Id) : IRequest<BudgetResponse>;
