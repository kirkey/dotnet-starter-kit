namespace FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

/// <summary>
/// Specification to find a Supplier by its unique Code.
/// </summary>
public sealed class SupplierByCodeSpec : Specification<Supplier>
{
    public SupplierByCodeSpec(string code)
    {
        Query.Where(s => s.Code == code);
    }
}
