using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Search.v1;

/// <summary>
/// Specification for searching member groups with filters and pagination.
/// </summary>
public class SearchMemberGroupsSpecs : EntitiesByPaginationFilterSpec<MemberGroup, MemberGroupResponse>
{
    public SearchMemberGroupsSpecs(SearchMemberGroupsCommand command)
        : base(command) =>
        Query
            .Include(g => g.Leader)
            .Include(g => g.Memberships)
            .OrderByDescending(g => g.CreatedOn, !command.HasOrderBy())
            .Where(g => g.Status == command.Status, !string.IsNullOrEmpty(command.Status))
            .Where(g => g.LoanOfficerId == command.LoanOfficerId!.Value, command.LoanOfficerId.HasValue)
            .Where(g => g.MeetingFrequency == command.MeetingFrequency, !string.IsNullOrEmpty(command.MeetingFrequency))
            .Where(g => g.MeetingDay == command.MeetingDay, !string.IsNullOrEmpty(command.MeetingDay))
            .Where(g => g.FormationDate >= command.FormationDateFrom!.Value, command.FormationDateFrom.HasValue)
            .Where(g => g.FormationDate <= command.FormationDateTo!.Value, command.FormationDateTo.HasValue);
}
