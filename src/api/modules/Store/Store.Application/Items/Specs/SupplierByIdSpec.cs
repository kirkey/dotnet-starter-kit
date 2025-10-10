namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

/// <summary>
/// Specification for finding a Supplier by its ID.
/// </summary>
public sealed class SupplierByIdSpec : Specification<Supplier>
{
    public SupplierByIdSpec(DefaultIdType supplierId)
    {
        Query.Where(s => s.Id == supplierId);
    }
}


