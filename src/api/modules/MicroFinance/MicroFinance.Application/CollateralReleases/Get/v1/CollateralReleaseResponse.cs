namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;

public sealed record CollateralReleaseResponse(
    Guid Id,
    Guid CollateralId,
    Guid LoanId,
    string ReleaseReference,
    string Status,
    DateOnly RequestDate,
    Guid RequestedById,
    string? ReleaseMethod,
    string? RecipientName,
    string? RecipientIdNumber,
    string? RecipientContact,
    DateOnly? ApprovedDate,
    Guid? ApprovedById,
    DateOnly? ReleasedDate,
    Guid? ReleasedById,
    string? RejectionReason,
    string? Notes,
    string? ReleaseDocumentPath,
    bool DocumentsReturned,
    bool RegistrationCleared);
