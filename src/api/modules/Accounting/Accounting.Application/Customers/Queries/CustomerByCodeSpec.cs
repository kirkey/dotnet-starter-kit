namespace Accounting.Application.Customers.Queries;

public sealed class CustomerByCodeSpec : Specification<Customer>, ISingleResultSpecification<Customer>
{
    public CustomerByCodeSpec(string code, DefaultIdType? excludeId = null)
    {
        var c = code.Trim();
        var lc = c.ToLowerInvariant();
        Query.Where(x => x.CustomerCode.ToLower() == lc);
        if (excludeId != null)
            Query.Where(x => x.Id != excludeId.Value);
    }
}
