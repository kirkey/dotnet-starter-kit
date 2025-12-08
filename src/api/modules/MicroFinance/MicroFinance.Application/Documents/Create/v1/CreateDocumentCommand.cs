using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Create.v1;

/// <summary>
/// Command to create a new document record.
/// </summary>
public sealed record CreateDocumentCommand(
    string Name,
    string DocumentType,
    string EntityType,
    DefaultIdType EntityId,
    string FilePath,
    long FileSizeBytes,
    string? MimeType = null,
    string? Category = null,
    string? Description = null,
    string? OriginalFileName = null) : IRequest<CreateDocumentResponse>;
