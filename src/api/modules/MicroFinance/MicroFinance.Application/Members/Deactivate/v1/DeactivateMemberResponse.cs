namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Deactivate.v1;

/// <summary>
/// Response from deactivating a member.
/// </summary>
public sealed record DeactivateMemberResponse(DefaultIdType Id, bool IsActive);
