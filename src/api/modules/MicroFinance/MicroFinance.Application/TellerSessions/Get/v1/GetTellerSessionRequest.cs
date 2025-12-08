using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;

public sealed record GetTellerSessionRequest(DefaultIdType Id) : IRequest<TellerSessionResponse>;
