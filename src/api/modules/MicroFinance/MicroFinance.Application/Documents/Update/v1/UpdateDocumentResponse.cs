namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Update.v1;

/// <summary>
/// Response after updating a document.
/// </summary>
public sealed record UpdateDocumentResponse(
    DefaultIdType Id,
    string Name,
    string DocumentType,
    string? Description,
    string Status);
