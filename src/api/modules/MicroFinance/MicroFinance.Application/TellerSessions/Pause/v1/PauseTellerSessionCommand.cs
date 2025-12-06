using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Pause.v1;

public sealed record PauseTellerSessionCommand(Guid Id) : IRequest<PauseTellerSessionResponse>;
