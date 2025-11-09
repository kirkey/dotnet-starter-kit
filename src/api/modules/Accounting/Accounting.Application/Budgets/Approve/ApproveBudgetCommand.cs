namespace Accounting.Application.Budgets.Approve;

/// <summary>
/// Command to approve a budget.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed record ApproveBudgetCommand(
    DefaultIdType BudgetId
) : IRequest<DefaultIdType>;
