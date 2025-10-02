using Accounting.Domain.Entities;

namespace Accounting.Application.Vendors.Queries;

public class VendorByNameSpec : Specification<Vendor>
{
    public VendorByNameSpec(string name)
    {
        Query.Where(v => v.Name == name);
    }
}

