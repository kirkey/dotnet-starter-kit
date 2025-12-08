using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Activate.v1;

/// <summary>
/// Command to activate a member group.
/// </summary>
public sealed record ActivateMemberGroupCommand(DefaultIdType Id) : IRequest<ActivateMemberGroupResponse>;
