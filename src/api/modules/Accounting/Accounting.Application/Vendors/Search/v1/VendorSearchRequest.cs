namespace Accounting.Application.Vendors.Search.v1;

/// <summary>
/// Request to search vendors with filtering and pagination.
/// </summary>
public class VendorSearchRequest : PaginationFilter, IRequest<PagedList<VendorSearchResponse>>
{
    // public string? Name { get; set; }
}
