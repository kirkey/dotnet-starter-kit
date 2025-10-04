namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

/// <summary>
/// Response returned after updating an item.
/// </summary>
/// <param name="Id">The identifier of the updated item.</param>
public record UpdateItemResponse(DefaultIdType Id);

