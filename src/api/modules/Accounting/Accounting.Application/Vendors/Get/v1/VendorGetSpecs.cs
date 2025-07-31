using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Vendors.Get.v1;

public class VendorGetSpecs : Specification<Vendor, VendorGetResponse>
{
    public VendorGetSpecs(DefaultIdType id)
    {
        Query
            .Where(v => v.Id == id);
    }
}

