using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Search.v1;

public class SearchMobileWalletsCommand : PaginationFilter, IRequest<PagedList<MobileWalletResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public string? Status { get; set; }
    public string? Tier { get; set; }
    public string? Provider { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsLinkedToBankAccount { get; set; }
}
