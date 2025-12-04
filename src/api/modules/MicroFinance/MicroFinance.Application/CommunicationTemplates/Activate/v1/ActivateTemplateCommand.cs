using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Activate.v1;

public sealed record ActivateTemplateCommand(Guid Id) : IRequest<ActivateTemplateResponse>;
