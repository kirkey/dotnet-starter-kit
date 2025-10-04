namespace FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;

/// <summary>
/// Response returned after updating a supplier.
/// </summary>
/// <param name="Id">The identifier of the updated supplier.</param>
public record UpdateSupplierResponse(DefaultIdType Id);
