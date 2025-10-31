using Accounting.Application.CostCenters.Queries;
using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Search.v1;

/// <summary>
/// Handler for searching cost centers with filters.
/// </summary>
public sealed class SearchCostCentersHandler(
    ILogger<SearchCostCentersHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<CostCenter> repository)
    : IRequestHandler<SearchCostCentersRequest, List<CostCenterResponse>>
{
    public async Task<List<CostCenterResponse>> Handle(SearchCostCentersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new CostCenterSearchSpec(request.Code, request.Name, request.CostCenterType, request.IsActive);
        var costCenters = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} cost centers", costCenters.Count);

        return costCenters.Select(cc => new CostCenterResponse
        {
            Id = cc.Id,
            Code = cc.Code,
            Name = cc.Name,
            CostCenterType = cc.CostCenterType.ToString(),
            IsActive = cc.IsActive,
            ParentCostCenterId = cc.ParentCostCenterId,
            ManagerId = cc.ManagerId,
            ManagerName = cc.ManagerName,
            BudgetAmount = cc.BudgetAmount
        }).ToList();
    }
}
