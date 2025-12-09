namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Update.v1;

/// <summary>
/// Response returned after successfully updating a collection action.
/// </summary>
/// <param name="Id">The unique identifier of the updated collection action.</param>
public sealed record UpdateCollectionActionResponse(DefaultIdType Id);
