using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;

public sealed record GetTellerSessionRequest(Guid Id) : IRequest<TellerSessionResponse>;
