namespace Accounting.Application.Budgets.Approve;

/// <summary>
/// Command to approve a budget.
/// </summary>
public sealed record ApproveBudgetCommand(
    DefaultIdType BudgetId,
    string ApprovedBy
) : IRequest<DefaultIdType>;
