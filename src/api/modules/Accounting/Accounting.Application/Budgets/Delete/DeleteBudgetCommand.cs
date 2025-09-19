namespace Accounting.Application.Budgets.Delete;

public sealed record DeleteBudgetCommand(DefaultIdType Id) : IRequest;
