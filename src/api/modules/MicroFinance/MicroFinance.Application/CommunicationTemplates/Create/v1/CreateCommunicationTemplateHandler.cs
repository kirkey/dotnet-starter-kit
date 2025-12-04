using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Create.v1;

public sealed class CreateCommunicationTemplateHandler(
    [FromKeyedServices("microfinance:communicationtemplates")] IRepository<CommunicationTemplate> repository,
    ILogger<CreateCommunicationTemplateHandler> logger)
    : IRequestHandler<CreateCommunicationTemplateCommand, CreateCommunicationTemplateResponse>
{
    public async Task<CreateCommunicationTemplateResponse> Handle(
        CreateCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var template = CommunicationTemplate.Create(
            request.Code,
            request.Name,
            request.Channel,
            request.Category,
            request.Body,
            request.Subject,
            request.Placeholders,
            request.Language,
            request.RequiresApproval);

        await repository.AddAsync(template, cancellationToken);

        logger.LogInformation("Communication template created: {TemplateId} with code {Code}",
            template.Id, request.Code);

        return new CreateCommunicationTemplateResponse(template.Id);
    }
}
