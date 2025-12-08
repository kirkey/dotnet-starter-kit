namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Reactivate.v1;

/// <summary>
/// Response after reactivating membership.
/// </summary>
public sealed record ReactivateMembershipResponse(DefaultIdType Id, string Status, string Message);
