using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Search.v1;

public class SearchGroupMembershipsCommand : PaginationFilter, IRequest<PagedList<GroupMembershipResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? GroupId { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
}
