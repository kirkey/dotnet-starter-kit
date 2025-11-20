using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Search.v1;

/// <summary>
/// Query request for searching and listing products with filtering and pagination.
/// Supports filtering by brand, price range, and inherits pagination capabilities.
/// </summary>
public class SearchProductsCommand : PaginationFilter, IRequest<PagedList<ProductResponse>>
{
    /// <summary>
    /// Optional filter by brand identifier.
    /// When provided, returns only products belonging to this brand.
    /// </summary>
    public DefaultIdType? BrandId { get; set; }

    /// <summary>
    /// Optional minimum price filter (inclusive).
    /// When provided, returns only products with price >= MinimumRate.
    /// </summary>
    public decimal? MinimumRate { get; set; }

    /// <summary>
    /// Optional maximum price filter (inclusive).
    /// When provided, returns only products with price <= MaximumRate.
    /// </summary>
    public decimal? MaximumRate { get; set; }
}
