using Ardalis.Specification;
using FSH.Starter.WebApi.Catalog.Domain;

namespace FSH.Starter.WebApi.Catalog.Application.Brands.Specifications;

/// <summary>
/// Specification to find a brand by its name (case-insensitive).
/// Used for uniqueness validation.
/// </summary>
public sealed class BrandByNameSpec : Specification<Brand>
{
    public BrandByNameSpec(string name)
    {
        Query.Where(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}

