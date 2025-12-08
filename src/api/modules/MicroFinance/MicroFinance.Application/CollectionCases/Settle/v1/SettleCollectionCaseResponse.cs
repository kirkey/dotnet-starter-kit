namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Settle.v1;

public sealed record SettleCollectionCaseResponse(
    DefaultIdType Id,
    string Status,
    DateOnly ClosedDate,
    string ClosureReason);
