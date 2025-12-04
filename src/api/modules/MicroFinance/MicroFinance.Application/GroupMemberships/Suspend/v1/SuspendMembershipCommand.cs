using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Suspend.v1;

/// <summary>
/// Command to suspend a group membership.
/// </summary>
public sealed record SuspendMembershipCommand(Guid Id, string? Reason = null) : IRequest<SuspendMembershipResponse>;
