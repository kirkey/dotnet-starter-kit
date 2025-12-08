using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Activate.v1;

/// <summary>
/// Command to activate an inactive member.
/// </summary>
public sealed record ActivateMemberCommand(DefaultIdType Id) : IRequest<ActivateMemberResponse>;
