namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Activate.v1;

/// <summary>
/// Response from activating a member.
/// </summary>
public sealed record ActivateMemberResponse(Guid Id, bool IsActive);
