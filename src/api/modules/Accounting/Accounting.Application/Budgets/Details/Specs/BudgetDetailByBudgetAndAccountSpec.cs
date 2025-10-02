namespace Accounting.Application.Budgets.Details.Specs;

public sealed class BudgetDetailByBudgetAndAccountSpec : Specification<BudgetDetail>
{
    public BudgetDetailByBudgetAndAccountSpec(DefaultIdType budgetId, DefaultIdType accountId)
    {
        Query.Where(x => x.BudgetId == budgetId && x.AccountId == accountId);
    }
}

