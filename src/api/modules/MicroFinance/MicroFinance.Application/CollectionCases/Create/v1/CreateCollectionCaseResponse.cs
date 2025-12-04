namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Create.v1;

public sealed record CreateCollectionCaseResponse(
    Guid Id,
    string CaseNumber,
    string Status,
    string Priority,
    string Classification);
