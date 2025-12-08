namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Activate.v1;

/// <summary>
/// Response returned after activating a member group.
/// </summary>
public sealed record ActivateMemberGroupResponse(
    DefaultIdType Id,
    string Status);
