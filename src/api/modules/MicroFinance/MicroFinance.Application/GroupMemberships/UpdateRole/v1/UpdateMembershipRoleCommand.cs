using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.UpdateRole.v1;

/// <summary>
/// Command to update a member's role in a group.
/// </summary>
public sealed record UpdateMembershipRoleCommand(DefaultIdType Id, string Role) : IRequest<UpdateMembershipRoleResponse>;
