using Accounting.Application.CostCenters.Queries;
using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Get;

/// <summary>
/// Handler for retrieving a cost center by ID.
/// </summary>
public class GetCostCenterHandler(
    [FromKeyedServices("accounting")] IReadRepository<CostCenter> repository)
    : IRequestHandler<GetCostCenterRequest, CostCenterResponse>
{
    public async Task<CostCenterResponse> Handle(
        GetCostCenterRequest request,
        CancellationToken cancellationToken)
    {
        var costCenter = await repository.FirstOrDefaultAsync(
            new CostCenterByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (costCenter is null)
        {
            throw new NotFoundException(
                $"{nameof(CostCenter)} with ID {request.Id} was not found.");
        }

        return new CostCenterResponse
        {
            Id = costCenter.Id,
            Code = costCenter.Code,
            Name = costCenter.Name,
            CostCenterType = costCenter.CostCenterType.ToString(),
            IsActive = costCenter.IsActive,
            ParentCostCenterId = costCenter.ParentCostCenterId,
            ManagerId = costCenter.ManagerId,
            ManagerName = costCenter.ManagerName,
            BudgetAmount = costCenter.BudgetAmount,
            ActualAmount = costCenter.ActualAmount,
            Location = costCenter.Location,
            StartDate = costCenter.StartDate,
            EndDate = costCenter.EndDate,
            Description = costCenter.Description
        };
    }
}

