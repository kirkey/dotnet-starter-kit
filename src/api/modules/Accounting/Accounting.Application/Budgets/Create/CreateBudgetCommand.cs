namespace Accounting.Application.Budgets.Create;

/// <summary>
/// Command to create a new Budget aggregate.
/// </summary>
/// <param name="Name">Human-friendly budget name. Required, max 256.</param>
/// <param name="PeriodId">Accounting period identifier. Required.</param>
/// <param name="FiscalYear">Fiscal year (1900-2100). Required.</param>
/// <param name="BudgetType">Budget type (e.g., Operating, Capital, Cash Flow). Required, max 32.</param>
/// <param name="Description">Optional description, max 1000.</param>
/// <param name="Notes">Optional notes, max 1000.</param>
public sealed record CreateBudgetCommand(
    string Name,
    DefaultIdType PeriodId,
    int FiscalYear,
    string BudgetType,
    string? Description = null,
    string? Notes = null
) : IRequest<CreateBudgetResponse>;
