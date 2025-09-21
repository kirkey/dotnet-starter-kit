using Accounting.Application.Budgets.Responses;

namespace Accounting.Application.Budgets.Search;

public sealed class SearchBudgetsQuery : PaginationFilter, IRequest<PagedList<BudgetResponse>>
{
    public string? Name { get; set; }
    public int? FiscalYear { get; set; }
    public string? Status { get; set; }
}
