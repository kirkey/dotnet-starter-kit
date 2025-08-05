using MediatR;
using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Get;

public record GetBudgetRequest(DefaultIdType Id) : IRequest<BudgetDto>;
