using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Verify.v1;

public sealed record VerifyTellerSessionCommand(
    Guid Id,
    Guid SupervisorUserId,
    string SupervisorName,
    string? Notes = null) : IRequest<VerifyTellerSessionResponse>;
