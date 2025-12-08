namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Archive.v1;

/// <summary>
/// Response after archiving a document.
/// </summary>
/// <param name="Id">The unique identifier of the archived document.</param>
/// <param name="Name">The document name.</param>
/// <param name="Status">The new status of the document.</param>
public sealed record ArchiveDocumentResponse(
    DefaultIdType Id,
    string Name,
    string Status);
