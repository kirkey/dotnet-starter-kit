using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Search.v1;

public class SearchShareAccountsCommand : PaginationFilter, IRequest<PagedList<ShareAccountResponse>>
{
    public string? AccountNumber { get; set; }
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? ShareProductId { get; set; }
    public string? Status { get; set; }
    public DateOnly? OpenedDateFrom { get; set; }
    public DateOnly? OpenedDateTo { get; set; }
}
