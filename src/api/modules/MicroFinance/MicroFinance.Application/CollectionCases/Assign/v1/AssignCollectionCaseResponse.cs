namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Assign.v1;

public sealed record AssignCollectionCaseResponse(
    DefaultIdType Id,
    DefaultIdType CollectorId,
    string Status,
    DateOnly? NextFollowUpDate);
