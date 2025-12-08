using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Update.v1;

/// <summary>
/// Command to update a member group's information.
/// </summary>
public sealed record UpdateMemberGroupCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    DefaultIdType? LeaderMemberId,
    DefaultIdType? LoanOfficerId,
    string? MeetingLocation,
    string? MeetingFrequency,
    string? MeetingDay,
    TimeOnly? MeetingTime,
    string? Notes) : IRequest<UpdateMemberGroupResponse>;
