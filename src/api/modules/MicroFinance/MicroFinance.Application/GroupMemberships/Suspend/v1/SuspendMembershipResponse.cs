namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Suspend.v1;

/// <summary>
/// Response after suspending membership.
/// </summary>
public sealed record SuspendMembershipResponse(Guid Id, string Status, string Message);
