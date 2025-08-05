using MediatR;

namespace Accounting.Application.Budgets.Delete;

public record DeleteBudgetRequest(DefaultIdType Id) : IRequest;
