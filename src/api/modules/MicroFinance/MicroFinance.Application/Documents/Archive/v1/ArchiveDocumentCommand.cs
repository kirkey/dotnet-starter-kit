using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Archive.v1;

/// <summary>
/// Command to archive a document.
/// </summary>
/// <param name="Id">The unique identifier of the document to archive.</param>
public sealed record ArchiveDocumentCommand(DefaultIdType Id) : IRequest<ArchiveDocumentResponse>;
