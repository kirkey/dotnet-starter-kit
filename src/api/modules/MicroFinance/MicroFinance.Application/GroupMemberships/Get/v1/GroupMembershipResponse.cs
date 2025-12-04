namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Get.v1;

public sealed record GroupMembershipResponse(
    Guid Id,
    Guid MemberId,
    Guid GroupId,
    DateOnly JoinDate,
    DateOnly? LeaveDate,
    string Role,
    string Status,
    string? Notes
);
