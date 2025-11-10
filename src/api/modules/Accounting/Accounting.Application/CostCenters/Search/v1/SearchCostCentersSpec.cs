using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Search.v1;

/// <summary>
/// Specification for searching cost centers with filtering and pagination.
/// Projects results to <see cref="CostCenterResponse"/>.
/// </summary>
public sealed class SearchCostCentersSpec : EntitiesByPaginationFilterSpec<CostCenter, CostCenterResponse>
{
    public SearchCostCentersSpec(SearchCostCentersRequest request) : base(request)
    {
        Query
            .OrderBy(c => c.Code, !request.HasOrderBy())
            .Where(c => c.Code.Contains(request.Code!), !string.IsNullOrWhiteSpace(request.Code))
            .Where(c => c.Name.Contains(request.Name!), !string.IsNullOrWhiteSpace(request.Name))
            .Where(c => c.CostCenterType.ToString() == request.CostCenterType!, !string.IsNullOrWhiteSpace(request.CostCenterType))
            .Where(c => c.IsActive == request.IsActive!.Value, request.IsActive.HasValue);
    }
}

