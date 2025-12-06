using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Resume.v1;

public sealed record ResumeTellerSessionCommand(Guid Id) : IRequest<ResumeTellerSessionResponse>;
