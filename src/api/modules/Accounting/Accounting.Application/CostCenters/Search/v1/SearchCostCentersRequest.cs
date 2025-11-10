using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Search.v1;

/// <summary>
/// Request to search for cost centers with optional filters and pagination.
/// </summary>
public sealed class SearchCostCentersRequest : PaginationFilter, IRequest<PagedList<CostCenterResponse>>
{
    public string? Code { get; init; }
    public string? Name { get; init; }
    public string? CostCenterType { get; init; }
    public bool? IsActive { get; init; }
}
