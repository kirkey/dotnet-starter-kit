namespace Accounting.Application.Budgets.Responses;

/// <summary>
/// Detailed response for a Budget aggregate.
/// </summary>
public sealed record BudgetResponse(
    DefaultIdType Id,
    string? Name,
    DefaultIdType PeriodId,
    string? PeriodName,
    int FiscalYear,
    string BudgetType,
    string Status,
    decimal TotalBudgetedAmount,
    decimal TotalActualAmount,
    DateTime? ApprovedOn,
    string? ApproverName,
    string? Description,
    string? Notes
);
