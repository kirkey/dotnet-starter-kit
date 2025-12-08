namespace FSH.Starter.WebApi.MicroFinance.Application.KycDocuments.Get.v1;

public sealed record KycDocumentResponse(
    DefaultIdType Id,
    DefaultIdType MemberId,
    string DocumentType,
    string? DocumentNumber,
    string FileName,
    string FilePath,
    string MimeType,
    long FileSize,
    DateOnly? IssueDate,
    DateOnly? ExpiryDate,
    string? IssuingAuthority,
    string Status,
    DateTime? VerifiedAt,
    DefaultIdType? VerifiedById,
    string? RejectionReason,
    bool IsPrimary);
