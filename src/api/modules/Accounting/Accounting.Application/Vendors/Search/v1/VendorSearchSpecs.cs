using Accounting.Domain.Entities;

namespace Accounting.Application.Vendors.Search.v1;
public class VendorSearchSpecs : EntitiesByPaginationFilterSpec<Vendor, VendorSearchResponse>
{
    public VendorSearchSpecs(VendorSearchQuery command)
        : base(command) =>
        Query
            .OrderBy(v => v.Name, !command.OrderBy.Any());
}
