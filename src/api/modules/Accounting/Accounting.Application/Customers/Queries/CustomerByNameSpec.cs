namespace Accounting.Application.Customers.Queries;

public sealed class CustomerByNameSpec : Specification<Customer>, ISingleResultSpecification<Customer>
{
    public CustomerByNameSpec(string name, DefaultIdType? excludeId = null)
    {
        var n = name.Trim();
        var ln = n.ToLowerInvariant();
        Query.Where(x => x.Name.ToLower() == ln);
        if (excludeId != null)
            Query.Where(x => x.Id != excludeId.Value);
    }
}
