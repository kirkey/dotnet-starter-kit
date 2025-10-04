namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;

/// <summary>
/// Response returned after updating an item-supplier relationship.
/// </summary>
/// <param name="Id">The identifier of the updated item-supplier relationship.</param>
public record UpdateItemSupplierResponse(DefaultIdType Id);

