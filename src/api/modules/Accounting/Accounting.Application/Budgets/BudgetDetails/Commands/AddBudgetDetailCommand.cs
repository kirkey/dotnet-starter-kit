namespace Accounting.Application.Budgets.BudgetDetails.Commands;

/// <summary>
/// Command to add a BudgetDetail to a Budget.
/// </summary>
public sealed record AddBudgetDetailCommand(
    DefaultIdType BudgetId,
    DefaultIdType AccountId,
    decimal BudgetedAmount,
    string? Description = null
) : IRequest<DefaultIdType>;
