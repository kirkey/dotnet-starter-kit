using Accounting.Application.ChartOfAccounts.Dtos;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

public class ChartOfAccountSearchRequest : PaginationFilter, IRequest<PagedList<ChartOfAccountDto>>
{
    public string? AccountCode { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
