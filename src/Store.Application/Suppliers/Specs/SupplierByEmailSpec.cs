namespace FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

/// <summary>
/// Specification to find a Supplier by its contact Email.
/// </summary>
public sealed class SupplierByEmailSpec : Specification<Supplier>
{
    public SupplierByEmailSpec(string email)
    {
        Query.Where(s => s.Email == email);
    }
}

