namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Search.v1;

/// <summary>
/// Specification for getting pay component by ID.
/// </summary>
public sealed class PayComponentByIdSpec : Specification<PayComponent>, ISingleResultSpecification<PayComponent>
{
    public PayComponentByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

/// <summary>
/// Specification for searching pay components with filters.
/// </summary>
public sealed class SearchPayComponentsSpec : Specification<PayComponent>
{
    public SearchPayComponentsSpec(SearchPayComponentsRequest request)
    {
        Query.OrderBy(x => x.ComponentType)
            .ThenBy(x => x.ComponentName);

        if (!string.IsNullOrWhiteSpace(request.ComponentType))
            Query.Where(x => x.ComponentType == request.ComponentType);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            Query.Where(x => x.ComponentName.Contains(request.SearchTerm));

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

/// <summary>
/// Specification for getting pay components by type.
/// </summary>
public sealed class PayComponentsByTypeSpec : Specification<PayComponent>
{
    public PayComponentsByTypeSpec(string componentType)
    {
        Query.Where(x => x.ComponentType == componentType && x.IsActive)
            .OrderBy(x => x.ComponentName);
    }
}

