namespace Accounting.Application.Budgets.BudgetDetails.Commands;

public class DeleteBudgetDetailCommand : IRequest
{
    public DefaultIdType BudgetId { get; set; }
    public DefaultIdType AccountId { get; set; }
}
