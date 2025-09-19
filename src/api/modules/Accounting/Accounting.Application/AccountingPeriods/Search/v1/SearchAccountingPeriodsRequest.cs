using Accounting.Application.AccountingPeriods.Dtos;

namespace Accounting.Application.AccountingPeriods.Search.v1;

public class SearchAccountingPeriodsQuery : PaginationFilter, IRequest<PagedList<AccountingPeriodResponse>>
{
    public string? Name { get; set; }
    public int? FiscalYear { get; set; }
    public bool IsClosed { get; set; }
}
