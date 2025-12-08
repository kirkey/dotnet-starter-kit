namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Close.v1;

/// <summary>
/// Response returned after closing a collection case.
/// </summary>
public sealed record CloseCollectionCaseResponse(
    DefaultIdType Id,
    string Status);
