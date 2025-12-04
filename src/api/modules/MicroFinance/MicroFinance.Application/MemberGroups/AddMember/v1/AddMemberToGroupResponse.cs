namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;

public sealed record AddMemberToGroupResponse(
    Guid MembershipId,
    Guid GroupId,
    Guid MemberId,
    string Role);
