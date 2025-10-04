namespace FSH.Starter.WebApi.Store.Application.PickLists.Update.v1;

/// <summary>
/// Response returned after updating a pick list.
/// </summary>
/// <param name="Id">The identifier of the updated pick list.</param>
public record UpdatePickListResponse(DefaultIdType Id);

