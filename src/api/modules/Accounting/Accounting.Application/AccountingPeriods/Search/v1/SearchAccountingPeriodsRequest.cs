using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Search.v1;

public class SearchAccountingPeriodsRequest : PaginationFilter, IRequest<PagedList<AccountingPeriodDto>>
{
    public string? Name { get; set; }
    public int? FiscalYear { get; set; }
    public bool IsClosed { get; set; }
}
