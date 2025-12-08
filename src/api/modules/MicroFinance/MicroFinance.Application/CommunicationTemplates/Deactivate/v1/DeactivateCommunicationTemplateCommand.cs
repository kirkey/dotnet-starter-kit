using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Deactivate.v1;

public sealed record DeactivateCommunicationTemplateCommand(
    DefaultIdType Id) : IRequest<DeactivateCommunicationTemplateResponse>;
