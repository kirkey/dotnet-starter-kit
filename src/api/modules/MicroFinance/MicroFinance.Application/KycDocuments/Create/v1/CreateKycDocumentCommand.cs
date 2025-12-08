using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Create.v1;

public sealed record CreateKycDocumentCommand(
    DefaultIdType MemberId,
    string DocumentType,
    string FileName,
    string FilePath,
    string MimeType,
    long FileSize,
    string? DocumentNumber = null,
    DateOnly? IssueDate = null,
    DateOnly? ExpiryDate = null,
    string? IssuingAuthority = null) : IRequest<CreateKycDocumentResponse>;
