namespace Accounting.Application.Vendors.Search.v1;

/// <summary>
/// Specification for searching vendors.
/// </summary>
public class VendorSearchSpecs : EntitiesByPaginationFilterSpec<Vendor, VendorSearchResponse>
{
    public VendorSearchSpecs(VendorSearchRequest request)
        : base(request) =>
        Query
            .OrderBy(v => v.Name, request.OrderBy?.Any() != true);
}
