namespace Accounting.Application.Budgets.BudgetDetails.Commands;

/// <summary>
/// Command to update a BudgetDetail for a Budget (identified by accountId).
/// </summary>
public sealed record UpdateBudgetDetailCommand(
    DefaultIdType BudgetId,
    DefaultIdType AccountId,
    decimal? BudgetedAmount,
    string? Description = null
) : IRequest<DefaultIdType>;
