namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Assign.v1;

public sealed record AssignCollectionCaseResponse(
    Guid Id,
    Guid CollectorId,
    string Status,
    DateOnly? NextFollowUpDate);
