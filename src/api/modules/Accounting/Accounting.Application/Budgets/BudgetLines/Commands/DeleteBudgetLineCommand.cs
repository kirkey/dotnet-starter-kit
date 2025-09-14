namespace Accounting.Application.Budgets.BudgetLines.Commands;

public class DeleteBudgetLineCommand : IRequest
{
    public DefaultIdType BudgetId { get; set; }
    public DefaultIdType AccountId { get; set; }
}

