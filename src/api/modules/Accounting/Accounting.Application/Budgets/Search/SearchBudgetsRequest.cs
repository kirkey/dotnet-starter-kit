using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Search;

public class SearchBudgetsRequest : PaginationFilter, IRequest<PagedList<BudgetDto>>
{
    public string? Name { get; set; }
    public int? FiscalYear { get; set; }
    public string? Status { get; set; }
}


