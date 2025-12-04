using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Search.v1;

/// <summary>
/// Command to search member groups with filters and pagination.
/// </summary>
public class SearchMemberGroupsCommand : PaginationFilter, IRequest<PagedList<MemberGroupResponse>>
{
    /// <summary>
    /// Filter by group status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by loan officer ID.
    /// </summary>
    public Guid? LoanOfficerId { get; set; }

    /// <summary>
    /// Filter by meeting frequency.
    /// </summary>
    public string? MeetingFrequency { get; set; }

    /// <summary>
    /// Filter by meeting day.
    /// </summary>
    public string? MeetingDay { get; set; }

    /// <summary>
    /// Filter by formation date range start.
    /// </summary>
    public DateOnly? FormationDateFrom { get; set; }

    /// <summary>
    /// Filter by formation date range end.
    /// </summary>
    public DateOnly? FormationDateTo { get; set; }
}
