namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;

public sealed record AddMemberToGroupResponse(
    DefaultIdType MembershipId,
    DefaultIdType GroupId,
    DefaultIdType MemberId,
    string Role);
