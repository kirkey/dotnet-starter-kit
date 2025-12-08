using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Deactivate.v1;

/// <summary>
/// Command to deactivate an active member.
/// </summary>
public sealed record DeactivateMemberCommand(DefaultIdType Id) : IRequest<DeactivateMemberResponse>;
