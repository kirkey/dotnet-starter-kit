using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.Dissolve.v1;

/// <summary>
/// Command to dissolve a member group.
/// </summary>
public sealed record DissolveMemberGroupCommand(
    DefaultIdType Id,
    string? Reason = null) : IRequest<DissolveMemberGroupResponse>;
