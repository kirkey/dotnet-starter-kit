namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Create.v1;

public sealed class CreateDocumentTemplateHandler(
    ILogger<CreateDocumentTemplateHandler> logger,
    [FromKeyedServices("hr:documenttemplates")] IRepository<DocumentTemplate> repository)
    : IRequestHandler<CreateDocumentTemplateCommand, CreateDocumentTemplateResponse>
{
    public async Task<CreateDocumentTemplateResponse> Handle(
        CreateDocumentTemplateCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var template = DocumentTemplate.Create(
            request.TemplateName,
            request.DocumentType,
            request.TemplateContent);

        if (!string.IsNullOrWhiteSpace(request.TemplateVariables))
            template.Update(templateVariables: request.TemplateVariables);

        if (!string.IsNullOrWhiteSpace(request.Description))
            template.Update(description: request.Description);

        await repository.AddAsync(template, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Document template created with ID {TemplateId}, Name {TemplateName}, Type {DocumentType}",
            template.Id,
            template.TemplateName,
            template.DocumentType);

        return new CreateDocumentTemplateResponse(template.Id);
    }
}

