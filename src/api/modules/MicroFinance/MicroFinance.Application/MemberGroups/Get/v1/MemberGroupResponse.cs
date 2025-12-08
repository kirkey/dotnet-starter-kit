namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Get.v1;

/// <summary>
/// Response containing member group details.
/// </summary>
/// <param name="Id">The unique identifier of the member group.</param>
/// <param name="Code">The unique group code.</param>
/// <param name="Name">The group name.</param>
/// <param name="Description">The group description.</param>
/// <param name="FormationDate">The date the group was formed.</param>
/// <param name="LeaderMemberId">The group leader member ID.</param>
/// <param name="LeaderName">The group leader's name.</param>
/// <param name="LoanOfficerId">The loan officer ID assigned to this group.</param>
/// <param name="MeetingLocation">The meeting location.</param>
/// <param name="MeetingFrequency">The meeting frequency (Weekly, Biweekly, Monthly).</param>
/// <param name="MeetingDay">The meeting day.</param>
/// <param name="MeetingTime">The meeting time.</param>
/// <param name="Status">The current status of the group.</param>
/// <param name="Notes">Internal notes about the group.</param>
/// <param name="MemberCount">The number of members in the group.</param>
public record MemberGroupResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string? Description,
    DateOnly FormationDate,
    DefaultIdType? LeaderMemberId,
    string? LeaderName,
    DefaultIdType? LoanOfficerId,
    string? MeetingLocation,
    string? MeetingFrequency,
    string? MeetingDay,
    TimeOnly? MeetingTime,
    string Status,
    string? Notes,
    int MemberCount);
