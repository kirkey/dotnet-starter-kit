namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Create.v1;

public sealed record CreateGeneratedDocumentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType DocumentTemplateId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EntityId,
    [property: DefaultValue("Employee")] string EntityType,
    [property: DefaultValue("<p>Generated content...</p>")] string GeneratedContent,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<CreateGeneratedDocumentResponse>;

