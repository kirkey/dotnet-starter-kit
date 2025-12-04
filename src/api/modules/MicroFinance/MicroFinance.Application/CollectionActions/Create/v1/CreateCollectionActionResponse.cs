namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Create.v1;

/// <summary>
/// Response after creating a collection action.
/// </summary>
public sealed record CreateCollectionActionResponse(
    Guid Id,
    string ActionType,
    string Outcome,
    DateTime ActionDateTime);
