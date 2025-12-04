using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;

public sealed class GetCommunicationTemplateHandler(
    [FromKeyedServices("microfinance:communicationtemplates")] IReadRepository<CommunicationTemplate> repository)
    : IRequestHandler<GetCommunicationTemplateRequest, CommunicationTemplateResponse>
{
    public async Task<CommunicationTemplateResponse> Handle(
        GetCommunicationTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var template = await repository.FirstOrDefaultAsync(
            new CommunicationTemplateByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Communication template {request.Id} not found");

        return new CommunicationTemplateResponse(
            template.Id,
            template.Code,
            template.Name,
            template.Channel,
            template.Category,
            template.Subject,
            template.Body,
            template.Placeholders,
            template.Language,
            template.RequiresApproval,
            template.Status);
    }
}
