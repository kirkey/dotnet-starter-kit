using MediatR;

namespace Accounting.Application.Budgets.Create;

public record CreateBudgetRequest(
    string Name,
    DefaultIdType PeriodId,
    int FiscalYear,
    string BudgetType,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
