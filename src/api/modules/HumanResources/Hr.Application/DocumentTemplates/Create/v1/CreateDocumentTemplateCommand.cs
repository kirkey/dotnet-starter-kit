namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Create.v1;

public sealed record CreateDocumentTemplateCommand(
    [property: DefaultValue("Employment Contract")] string TemplateName,
    [property: DefaultValue("EmploymentContract")] string DocumentType,
    [property: DefaultValue("<p>This is an employment contract...</p>")] string TemplateContent,
    [property: DefaultValue(null)] string? TemplateVariables = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<CreateDocumentTemplateResponse>;

