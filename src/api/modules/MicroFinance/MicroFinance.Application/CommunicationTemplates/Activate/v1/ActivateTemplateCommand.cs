using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Activate.v1;

public sealed record ActivateTemplateCommand(DefaultIdType Id) : IRequest<ActivateTemplateResponse>;
