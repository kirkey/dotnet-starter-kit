namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Create.v1;

/// <summary>
/// Response after creating a document.
/// </summary>
public sealed record CreateDocumentResponse(
    Guid Id,
    string Name,
    string DocumentType,
    string EntityType,
    Guid EntityId);
