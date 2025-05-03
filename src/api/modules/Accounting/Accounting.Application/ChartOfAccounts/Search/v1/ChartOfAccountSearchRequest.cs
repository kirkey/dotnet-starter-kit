using Accounting.Application.ChartOfAccounts.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.ChartOfAccounts.Search.v1;

public class ChartOfAccountSearchRequest : PaginationFilter, IRequest<PagedList<ChartOfAccountDto>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
