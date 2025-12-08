using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Verify.v1;

/// <summary>
/// Command to verify a document.
/// </summary>
public sealed record VerifyDocumentCommand(DefaultIdType DocumentId, DefaultIdType VerifiedById) : IRequest<VerifyDocumentResponse>;
