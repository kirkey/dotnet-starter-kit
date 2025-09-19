using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Search;

public class SearchBudgetsQuery : PaginationFilter, IRequest<PagedList<BudgetDto>>
{
    public DefaultIdType? ProjectId { get; set; }
    public string? Name { get; set; }
}
