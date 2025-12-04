namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Get.v1;

/// <summary>
/// Response containing collection action details.
/// </summary>
public sealed record CollectionActionResponse(
    Guid Id,
    Guid CollectionCaseId,
    Guid LoanId,
    string ActionType,
    DateTime ActionDateTime,
    Guid PerformedById,
    string? ContactMethod,
    string? PhoneNumberCalled,
    string? ContactPerson,
    string Outcome,
    string? Description,
    decimal? PromisedAmount,
    DateOnly? PromisedDate,
    DateOnly? NextFollowUpDate);
