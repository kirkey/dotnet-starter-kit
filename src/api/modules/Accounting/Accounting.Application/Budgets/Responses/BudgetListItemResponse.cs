namespace Accounting.Application.Budgets.Responses;

/// <summary>
/// Lightweight response shape for listing/searching budgets.
/// </summary>
public sealed record BudgetListItemResponse(
    DefaultIdType Id,
    string? Name,
    int FiscalYear,
    string BudgetType,
    string Status,
    decimal TotalBudgetedAmount,
    decimal TotalActualAmount
);

