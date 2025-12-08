using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;

public sealed record GetUssdSessionRequest(DefaultIdType Id) : IRequest<UssdSessionResponse>;
