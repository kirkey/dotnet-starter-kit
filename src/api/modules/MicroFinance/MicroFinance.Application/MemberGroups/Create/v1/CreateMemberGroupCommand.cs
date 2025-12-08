using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Create.v1;

public sealed record CreateMemberGroupCommand(
    [property: DefaultValue("GRP001")] string Code,
    [property: DefaultValue("Village Savings Group A")] string Name,
    [property: DefaultValue("Community savings and loan group for local farmers")] string? Description,
    [property: DefaultValue(null)] DateOnly? FormationDate,
    [property: DefaultValue(null)] DefaultIdType? LeaderMemberId,
    [property: DefaultValue(null)] DefaultIdType? LoanOfficerId,
    [property: DefaultValue("Main Street, Village Center")] string? MeetingLocation,
    [property: DefaultValue("Weekly")] string? MeetingFrequency,
    [property: DefaultValue("Monday")] string? MeetingDay,
    [property: DefaultValue(null)] TimeOnly? MeetingTime) : IRequest<CreateMemberGroupResponse>;
