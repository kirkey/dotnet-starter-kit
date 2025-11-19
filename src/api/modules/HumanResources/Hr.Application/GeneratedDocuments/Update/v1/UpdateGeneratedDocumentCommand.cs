namespace FSH.Starter.WebApi.HumanResources.Application.GeneratedDocuments.Update.v1;

public sealed record UpdateGeneratedDocumentCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] string? FilePath = null,
    [property: DefaultValue(null)] string? SignedBy = null,
    [property: DefaultValue(null)] string? Notes = null) : IRequest<UpdateGeneratedDocumentResponse>;

