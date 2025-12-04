using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Update.v1;

/// <summary>
/// Command to update a member group's information.
/// </summary>
public sealed record UpdateMemberGroupCommand(
    Guid Id,
    string? Name,
    string? Description,
    Guid? LeaderMemberId,
    Guid? LoanOfficerId,
    string? MeetingLocation,
    string? MeetingFrequency,
    string? MeetingDay,
    TimeOnly? MeetingTime,
    string? Notes) : IRequest<UpdateMemberGroupResponse>;
