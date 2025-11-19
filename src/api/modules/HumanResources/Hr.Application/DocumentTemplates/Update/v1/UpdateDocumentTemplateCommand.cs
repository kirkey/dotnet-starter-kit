namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Update.v1;

public sealed record UpdateDocumentTemplateCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? TemplateName = null,
    [property: DefaultValue(null)] string? TemplateContent = null,
    [property: DefaultValue(null)] string? TemplateVariables = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<UpdateDocumentTemplateResponse>;

