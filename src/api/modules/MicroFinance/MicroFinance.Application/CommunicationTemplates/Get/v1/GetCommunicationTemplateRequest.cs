using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;

public sealed record GetCommunicationTemplateRequest(Guid Id) : IRequest<CommunicationTemplateResponse>;
