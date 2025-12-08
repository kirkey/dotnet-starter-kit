using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.MarkExpired.v1;

/// <summary>
/// Command to mark a document as expired.
/// </summary>
/// <param name="Id">The unique identifier of the document to mark as expired.</param>
public sealed record MarkExpiredDocumentCommand(DefaultIdType Id) : IRequest<MarkExpiredDocumentResponse>;
