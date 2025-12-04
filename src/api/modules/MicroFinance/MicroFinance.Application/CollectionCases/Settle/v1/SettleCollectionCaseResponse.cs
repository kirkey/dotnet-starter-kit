namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Settle.v1;

public sealed record SettleCollectionCaseResponse(
    Guid Id,
    string Status,
    DateOnly ClosedDate,
    string ClosureReason);
