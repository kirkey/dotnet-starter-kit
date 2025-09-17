namespace Accounting.Application.Budgets.Update;

public record UpdateBudgetRequest(
    DefaultIdType Id,
    int FiscalYear,
    string? Name = null,
    string? BudgetType = null,
    string? Status = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
