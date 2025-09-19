using Accounting.Application.ChartOfAccounts.Responses;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

public class SearchChartOfAccountQuery : PaginationFilter, IRequest<PagedList<ChartOfAccountResponse>>
{
    public string? AccountCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
