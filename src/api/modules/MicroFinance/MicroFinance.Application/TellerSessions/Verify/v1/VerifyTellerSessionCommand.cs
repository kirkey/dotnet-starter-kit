using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Verify.v1;

public sealed record VerifyTellerSessionCommand(
    DefaultIdType Id,
    DefaultIdType SupervisorUserId,
    string SupervisorName,
    string? Notes = null) : IRequest<VerifyTellerSessionResponse>;
