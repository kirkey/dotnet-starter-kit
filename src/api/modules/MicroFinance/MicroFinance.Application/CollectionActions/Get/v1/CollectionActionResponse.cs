namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Get.v1;

/// <summary>
/// Response containing collection action details.
/// </summary>
public sealed record CollectionActionResponse(
    DefaultIdType Id,
    DefaultIdType CollectionCaseId,
    DefaultIdType LoanId,
    string ActionType,
    DateTime ActionDateTime,
    DefaultIdType PerformedById,
    string? ContactMethod,
    string? PhoneNumberCalled,
    string? ContactPerson,
    string Outcome,
    string? Description,
    decimal? PromisedAmount,
    DateOnly? PromisedDate,
    DateOnly? NextFollowUpDate);
