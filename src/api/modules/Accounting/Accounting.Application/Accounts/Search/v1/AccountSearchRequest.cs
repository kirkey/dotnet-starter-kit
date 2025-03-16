using FSH.Framework.Core.Paging;
using Accounting.Application.Accounts.Get.v1;
using MediatR;

namespace Accounting.Application.Accounts.Search.v1;

public class AccountSearchRequest : PaginationFilter, IRequest<PagedList<AccountResponse>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
