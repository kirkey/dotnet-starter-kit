using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Search.v1;

public class SearchGroupMembershipsSpecs : EntitiesByPaginationFilterSpec<GroupMembership, GroupMembershipResponse>
{
    public SearchGroupMembershipsSpecs(SearchGroupMembershipsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(gm => gm.JoinDate, !command.HasOrderBy())
            .Where(gm => gm.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(gm => gm.GroupId == command.GroupId!.Value, command.GroupId.HasValue)
            .Where(gm => gm.Role == command.Role, !string.IsNullOrWhiteSpace(command.Role))
            .Where(gm => gm.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status));
}
