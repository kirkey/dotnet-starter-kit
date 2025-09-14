namespace Accounting.Application.Budgets.Update;

public record UpdateBudgetRequest(
    DefaultIdType Id,
    string? Name = null,
    string? BudgetType = null,
    string? Status = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
