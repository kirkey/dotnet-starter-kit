namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Update.v1;

public sealed class UpdateDocumentTemplateHandler(
    ILogger<UpdateDocumentTemplateHandler> logger,
    [FromKeyedServices("hr:documenttemplates")] IRepository<DocumentTemplate> repository)
    : IRequestHandler<UpdateDocumentTemplateCommand, UpdateDocumentTemplateResponse>
{
    public async Task<UpdateDocumentTemplateResponse> Handle(
        UpdateDocumentTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var template = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (template is null)
            throw new DocumentTemplateNotFoundException(request.Id);

        template.Update(
            request.TemplateName,
            request.TemplateContent,
            request.TemplateVariables,
            request.Description);

        await repository.UpdateAsync(template, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Document template {TemplateId} updated successfully", template.Id);

        return new UpdateDocumentTemplateResponse(template.Id);
    }
}

