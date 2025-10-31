using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Search.v1;

/// <summary>
/// Request to search for cost centers with optional filters.
/// </summary>
public record SearchCostCentersRequest(
    string? Code = null,
    string? Name = null,
    string? CostCenterType = null,
    bool? IsActive = null) : IRequest<List<CostCenterResponse>>;
