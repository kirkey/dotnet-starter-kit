using Accounting.Application.Budgets.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.Budgets.Search;

public class SearchBudgetsRequest : PaginationFilter, IRequest<PagedList<BudgetDto>>
{
    public string? Name { get; set; }
    public int? FiscalYear { get; set; }
    public string? Status { get; set; }
}


