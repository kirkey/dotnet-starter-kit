using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;

public sealed record GetLegalActionRequest(Guid Id) : IRequest<LegalActionResponse>;
