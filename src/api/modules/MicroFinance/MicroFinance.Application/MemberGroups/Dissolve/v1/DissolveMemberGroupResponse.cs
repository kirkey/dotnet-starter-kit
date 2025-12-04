namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dissolve.v1;

/// <summary>
/// Response returned after dissolving a member group.
/// </summary>
public sealed record DissolveMemberGroupResponse(
    Guid Id,
    string Status);
