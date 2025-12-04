using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Deactivate.v1;

/// <summary>
/// Command to deactivate a member group.
/// </summary>
public sealed record DeactivateMemberGroupCommand(Guid Id) : IRequest<DeactivateMemberGroupResponse>;
