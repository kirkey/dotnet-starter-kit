using Accounting.Application.Budgets.Dtos;
using Accounting.Domain;
using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;

namespace Accounting.Application.Budgets.Search;

public sealed class SearchBudgetsSpec : EntitiesByPaginationFilterSpec<Budget, BudgetDto>
{
    public SearchBudgetsSpec(SearchBudgetsRequest request) : base(request)
    {
        Query
            .OrderBy(b => b.Name!, !request.HasOrderBy())
            .Where(b => b.Name!.Contains(request.Name!), !string.IsNullOrEmpty(request.Name))
            .Where(b => b.FiscalYear == request.FiscalYear, request.FiscalYear.HasValue)
            .Where(b => b.Status!.Contains(request.Status!), !string.IsNullOrEmpty(request.Status));
    }
}


