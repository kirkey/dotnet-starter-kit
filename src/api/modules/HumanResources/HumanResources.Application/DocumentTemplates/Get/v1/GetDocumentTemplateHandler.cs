using FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;

public sealed class GetDocumentTemplateHandler(
    [FromKeyedServices("hr:documenttemplates")] IReadRepository<DocumentTemplate> repository)
    : IRequestHandler<GetDocumentTemplateRequest, DocumentTemplateResponse>
{
    public async Task<DocumentTemplateResponse> Handle(
        GetDocumentTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var template = await repository
            .FirstOrDefaultAsync(new DocumentTemplateByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (template is null)
            throw new DocumentTemplateNotFoundException(request.Id);

        return new DocumentTemplateResponse
        {
            Id = template.Id,
            TemplateName = template.TemplateName,
            DocumentType = template.DocumentType,
            TemplateContent = template.TemplateContent,
            TemplateVariables = template.TemplateVariables,
            Description = template.Description,
            Version = template.Version,
            IsActive = template.IsActive
        };
    }
}

