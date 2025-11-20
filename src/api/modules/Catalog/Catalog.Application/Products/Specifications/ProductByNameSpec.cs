using Ardalis.Specification;
using FSH.Starter.WebApi.Catalog.Domain;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Specifications;

/// <summary>
/// Specification to find a product by its name (case-insensitive).
/// Used for uniqueness validation.
/// </summary>
public sealed class ProductByNameSpec : Specification<Product>
{
    public ProductByNameSpec(string name)
    {
        Query.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}

