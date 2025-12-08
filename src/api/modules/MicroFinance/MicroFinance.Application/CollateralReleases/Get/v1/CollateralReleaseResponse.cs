namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;

public sealed record CollateralReleaseResponse(
    DefaultIdType Id,
    DefaultIdType CollateralId,
    DefaultIdType LoanId,
    string ReleaseReference,
    string Status,
    DateOnly RequestDate,
    DefaultIdType RequestedById,
    string? ReleaseMethod,
    string? RecipientName,
    string? RecipientIdNumber,
    string? RecipientContact,
    DateOnly? ApprovedDate,
    DefaultIdType? ApprovedById,
    DateOnly? ReleasedDate,
    DefaultIdType? ReleasedById,
    string? RejectionReason,
    string? Notes,
    string? ReleaseDocumentPath,
    bool DocumentsReturned,
    bool RegistrationCleared);
