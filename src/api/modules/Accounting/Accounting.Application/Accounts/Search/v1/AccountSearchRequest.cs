using Accounting.Application.Accounts.Dtos;
using FSH.Framework.Core.Paging;
using MediatR;

namespace Accounting.Application.Accounts.Search.v1;

public class AccountSearchRequest : PaginationFilter, IRequest<PagedList<AccountDto>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
