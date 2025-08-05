namespace Accounting.Application.Budgets.Dtos;

public record BudgetDto(
    DefaultIdType Id,
    string Name,
    DefaultIdType PeriodId,
    int FiscalYear,
    string BudgetType,
    string Status,
    decimal TotalBudgetedAmount,
    decimal TotalActualAmount,
    DateTime? ApprovedDate,
    string? ApprovedBy,
    string? Description,
    string? Notes);

public record BudgetLineDto(
    DefaultIdType Id,
    DefaultIdType BudgetId,
    DefaultIdType AccountId,
    decimal BudgetedAmount,
    decimal ActualAmount,
    string? Description);
