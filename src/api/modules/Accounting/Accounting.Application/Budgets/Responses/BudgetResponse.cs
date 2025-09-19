namespace Accounting.Application.Budgets.Responses;

/// <summary>
/// Detailed response for a Budget aggregate.
/// </summary>
public sealed record BudgetResponse(
    DefaultIdType Id,
    string? Name,
    DefaultIdType PeriodId,
    int FiscalYear,
    string BudgetType,
    string Status,
    decimal TotalBudgetedAmount,
    decimal TotalActualAmount,
    DateTime? ApprovedDate,
    string? ApprovedBy,
    string? Description,
    string? Notes
);

