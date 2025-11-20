using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Catalog.Application.Brands.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Search.v1;

/// <summary>
/// Query request for searching and listing brands with filtering and pagination.
/// Inherits pagination, sorting, and filtering capabilities from PaginationFilter.
/// </summary>
public class SearchBrandsCommand : PaginationFilter, IRequest<PagedList<BrandResponse>>
{
    /// <summary>
    /// Optional filter by brand name (case-insensitive partial match).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Optional filter by brand description (case-insensitive partial match).
    /// </summary>
    public string? Description { get; set; }
}
