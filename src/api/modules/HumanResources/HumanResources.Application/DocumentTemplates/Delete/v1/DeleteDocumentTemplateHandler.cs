namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Delete.v1;

public sealed class DeleteDocumentTemplateHandler(
    ILogger<DeleteDocumentTemplateHandler> logger,
    [FromKeyedServices("hr:documenttemplates")] IRepository<DocumentTemplate> repository)
    : IRequestHandler<DeleteDocumentTemplateCommand, DeleteDocumentTemplateResponse>
{
    public async Task<DeleteDocumentTemplateResponse> Handle(
        DeleteDocumentTemplateCommand request,
        CancellationToken cancellationToken)
    {
        var template = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (template is null)
            throw new DocumentTemplateNotFoundException(request.Id);

        await repository.DeleteAsync(template, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Document template {TemplateId} deleted successfully", template.Id);

        return new DeleteDocumentTemplateResponse(template.Id);
    }
}

