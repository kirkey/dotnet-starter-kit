namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Create.v1;

public sealed record CreateCollectionCaseResponse(
    DefaultIdType Id,
    string CaseNumber,
    string Status,
    string Priority,
    string Classification);
