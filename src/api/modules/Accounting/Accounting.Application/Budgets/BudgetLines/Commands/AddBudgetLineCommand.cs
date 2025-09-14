namespace Accounting.Application.Budgets.BudgetLines.Commands;

public class AddBudgetLineCommand : IRequest<DefaultIdType>
{
    public DefaultIdType BudgetId { get; set; }
    public DefaultIdType AccountId { get; set; }
    public decimal BudgetedAmount { get; set; }
    public string? Description { get; set; }
}

