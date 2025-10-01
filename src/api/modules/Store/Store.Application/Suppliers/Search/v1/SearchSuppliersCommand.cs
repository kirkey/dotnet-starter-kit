using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;

/// <summary>
/// Paginated search command for Suppliers with optional filters.
/// </summary>
public class SearchSuppliersCommand : PaginationFilter, IRequest<PagedList<SupplierResponse>>
{
    /// <summary>
    /// Optional name contains filter.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Optional exact code filter.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Optional city filter.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Optional country filter.
    /// </summary>
    public string? Country { get; set; }
}
