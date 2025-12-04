using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Activate.v1;

public sealed class ActivateTemplateHandler(
    [FromKeyedServices("microfinance:communicationtemplates")] IRepository<CommunicationTemplate> repository,
    ILogger<ActivateTemplateHandler> logger)
    : IRequestHandler<ActivateTemplateCommand, ActivateTemplateResponse>
{
    public async Task<ActivateTemplateResponse> Handle(
        ActivateTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var template = await repository.FirstOrDefaultAsync(
            new CommunicationTemplateByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Communication template {request.Id} not found");

        template.Activate();
        await repository.UpdateAsync(template, cancellationToken);

        logger.LogInformation("Communication template activated: {TemplateId}", template.Id);

        return new ActivateTemplateResponse(template.Id, template.Status);
    }
}
