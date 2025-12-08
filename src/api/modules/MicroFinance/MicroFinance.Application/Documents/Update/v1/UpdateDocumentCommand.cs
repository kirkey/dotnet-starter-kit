using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Update.v1;

/// <summary>
/// Command to update a document.
/// </summary>
public sealed record UpdateDocumentCommand(
    DefaultIdType Id,
    string? Name = null,
    string? Description = null,
    string? Tags = null,
    int? DisplayOrder = null) : IRequest<UpdateDocumentResponse>;
