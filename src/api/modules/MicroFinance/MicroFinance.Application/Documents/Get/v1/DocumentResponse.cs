namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Get.v1;

/// <summary>
/// Response containing document details.
/// </summary>
public sealed record DocumentResponse(
    DefaultIdType Id,
    string Name,
    string DocumentType,
    string? Category,
    string Status,
    string EntityType,
    DefaultIdType EntityId,
    string FilePath,
    string? MimeType,
    long FileSizeBytes,
    string? Description,
    string? OriginalFileName,
    DateOnly? IssueDate,
    DateOnly? ExpiryDate,
    string? IssuingAuthority,
    string? DocumentNumber,
    bool IsVerified,
    DateTimeOffset? VerifiedAt,
    bool IsRequired);
