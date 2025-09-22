namespace Accounting.Application.BudgetDetails.Specs;

public sealed class BudgetDetailsByBudgetIdSpec : Specification<BudgetDetail>
{
    public BudgetDetailsByBudgetIdSpec(DefaultIdType budgetId)
    {
        Query.Where(x => x.BudgetId == budgetId);
    }
}

