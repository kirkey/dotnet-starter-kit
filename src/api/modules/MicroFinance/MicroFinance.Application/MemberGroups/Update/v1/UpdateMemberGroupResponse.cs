namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Update.v1;

/// <summary>
/// Response returned after updating a member group.
/// </summary>
public sealed record UpdateMemberGroupResponse(
    Guid Id,
    string Name,
    string Status);
