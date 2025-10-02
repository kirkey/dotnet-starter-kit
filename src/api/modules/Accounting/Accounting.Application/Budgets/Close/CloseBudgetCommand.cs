namespace Accounting.Application.Budgets.Close;

/// <summary>
/// Command to close an active budget.
/// </summary>
public sealed record CloseBudgetCommand(
    DefaultIdType BudgetId
) : IRequest<DefaultIdType>;
