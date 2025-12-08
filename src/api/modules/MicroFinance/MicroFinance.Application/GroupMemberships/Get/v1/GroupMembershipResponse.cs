namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;

public sealed record GroupMembershipResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    DefaultIdType GroupId,
    DateOnly JoinDate,
    DateOnly? LeaveDate,
    string Role,
    string Status,
    string? Notes
);
