using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.Search;

public sealed class SearchBudgetsSpec : EntitiesByPaginationFilterSpec<Budget, BudgetResponse>
{
    public SearchBudgetsSpec(SearchBudgetsQuery request) : base(request)
    {
        Query
            .OrderBy(b => b.Name!, !request.HasOrderBy())
            .Where(b => b.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(b => b.FiscalYear == request.FiscalYear, request.FiscalYear.HasValue)
            .Where(b => b.Status!.Contains(request.Status!), !string.IsNullOrEmpty(request.Status));
    }
}
