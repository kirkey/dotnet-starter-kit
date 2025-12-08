using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Deactivate.v1;

public sealed class DeactivateCommunicationTemplateHandler(
    [FromKeyedServices("microfinance:communicationtemplates")] IRepository<CommunicationTemplate> repository,
    ILogger<DeactivateCommunicationTemplateHandler> logger)
    : IRequestHandler<DeactivateCommunicationTemplateCommand, DeactivateCommunicationTemplateResponse>
{
    public async Task<DeactivateCommunicationTemplateResponse> Handle(
        DeactivateCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var template = await repository.FirstOrDefaultAsync(
            new CommunicationTemplateByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (template is null)
        {
            throw new NotFoundException($"Communication template with ID {request.Id} not found.");
        }

        template.Deactivate();

        await repository.UpdateAsync(template, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Communication template deactivated: {TemplateId}", request.Id);

        return new DeactivateCommunicationTemplateResponse(template.Id);
    }
}
