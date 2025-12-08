using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Reactivate.v1;

/// <summary>
/// Command to reactivate a suspended group membership.
/// </summary>
public sealed record ReactivateMembershipCommand(DefaultIdType Id) : IRequest<ReactivateMembershipResponse>;
