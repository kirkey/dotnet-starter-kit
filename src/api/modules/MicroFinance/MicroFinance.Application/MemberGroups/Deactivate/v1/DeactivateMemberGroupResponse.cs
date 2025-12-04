namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Deactivate.v1;

/// <summary>
/// Response returned after deactivating a member group.
/// </summary>
public sealed record DeactivateMemberGroupResponse(
    Guid Id,
    string Status);
