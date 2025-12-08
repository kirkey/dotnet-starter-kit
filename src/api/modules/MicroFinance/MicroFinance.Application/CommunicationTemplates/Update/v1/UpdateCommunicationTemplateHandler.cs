using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Update.v1;

public sealed class UpdateCommunicationTemplateHandler(
    [FromKeyedServices("microfinance:communicationtemplates")] IRepository<CommunicationTemplate> repository,
    ILogger<UpdateCommunicationTemplateHandler> logger)
    : IRequestHandler<UpdateCommunicationTemplateCommand, UpdateCommunicationTemplateResponse>
{
    public async Task<UpdateCommunicationTemplateResponse> Handle(
        UpdateCommunicationTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var template = await repository.FirstOrDefaultAsync(
            new CommunicationTemplateByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (template is null)
        {
            throw new NotFoundException($"Communication template with ID {request.Id} not found.");
        }

        template.Update(
            request.Name,
            request.Subject,
            request.Body,
            request.Placeholders,
            request.RequiresApproval,
            request.Notes);

        await repository.UpdateAsync(template, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Communication template updated: {TemplateId}", request.Id);

        return new UpdateCommunicationTemplateResponse(template.Id);
    }
}
