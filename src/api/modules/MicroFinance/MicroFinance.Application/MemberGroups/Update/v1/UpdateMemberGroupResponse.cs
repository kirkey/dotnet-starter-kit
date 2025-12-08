namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Update.v1;

/// <summary>
/// Response returned after updating a member group.
/// </summary>
public sealed record UpdateMemberGroupResponse(
    DefaultIdType Id,
    string Name,
    string Status);
