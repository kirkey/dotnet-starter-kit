using Accounting.Domain.Entities;

namespace Accounting.Application.Budgets.Queries;

public sealed class BudgetDetailsByBudgetIdSpec : Specification<BudgetDetail, List<Responses.BudgetDetailResponse>>
{
    public BudgetDetailsByBudgetIdSpec(DefaultIdType budgetId)
    {
        Query.Where(b => b.BudgetId == budgetId);
    }
}
