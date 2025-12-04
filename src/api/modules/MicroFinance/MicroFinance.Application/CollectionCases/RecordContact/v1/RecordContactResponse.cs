namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.RecordContact.v1;

public sealed record RecordContactResponse(
    Guid Id,
    DateOnly LastContactDate,
    DateOnly? NextFollowUpDate,
    int ContactAttempts,
    string Status);
