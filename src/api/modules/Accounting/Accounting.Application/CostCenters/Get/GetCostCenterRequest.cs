using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Get;

/// <summary>
/// Request to get a cost center by ID.
/// </summary>
public record GetCostCenterRequest(DefaultIdType Id) : IRequest<CostCenterResponse>;
