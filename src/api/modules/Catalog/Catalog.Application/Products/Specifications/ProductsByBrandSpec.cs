using Ardalis.Specification;
using FSH.Starter.WebApi.Catalog.Domain;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Specifications;

/// <summary>
/// Specification to find all products belonging to a specific brand.
/// Used for delete validation and queries.
/// </summary>
public sealed class ProductsByBrandSpec : Specification<Product>
{
    public ProductsByBrandSpec(DefaultIdType brandId)
    {
        Query.Where(p => p.BrandId == brandId);
    }
}

