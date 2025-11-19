namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;

public sealed record DocumentTemplateResponse
{
    public DefaultIdType Id { get; init; }
    public string TemplateName { get; init; } = default!;
    public string DocumentType { get; init; } = default!;
    public string TemplateContent { get; init; } = default!;
    public string? TemplateVariables { get; init; }
    public string? Description { get; init; }
    public int Version { get; init; }
    public bool IsActive { get; init; }
}

