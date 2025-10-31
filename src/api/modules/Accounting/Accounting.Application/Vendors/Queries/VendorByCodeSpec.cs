namespace Accounting.Application.Vendors.Queries;

public class VendorByCodeSpec : Specification<Vendor>
{
    public VendorByCodeSpec(string code)
    {
        Query.Where(v => v.VendorCode == code);
    }
}

