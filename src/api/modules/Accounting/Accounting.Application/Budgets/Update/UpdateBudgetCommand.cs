namespace Accounting.Application.Budgets.Update;

/// <summary>
/// Command to update a Budget aggregate.
/// </summary>
public sealed record UpdateBudgetCommand(
    DefaultIdType Id,
    DefaultIdType PeriodId,
    int FiscalYear,
    string? Name = null,
    string? BudgetType = null,
    string? Status = null,
    string? Description = null,
    string? Notes = null
) : IRequest<UpdateBudgetResponse>;
