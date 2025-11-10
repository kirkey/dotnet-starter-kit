using Accounting.Application.CostCenters.Queries;
using Accounting.Application.CostCenters.Responses;

namespace Accounting.Application.CostCenters.Get;

/// <summary>
/// Handler for retrieving a cost center by ID.
/// </summary>
public sealed class GetCostCenterHandler(
    [FromKeyedServices("accounting:costCenters")] IReadRepository<CostCenter> repository)
    : IRequestHandler<GetCostCenterRequest, CostCenterResponse>
{
    public async Task<CostCenterResponse> Handle(
        GetCostCenterRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new CostCenterByIdSpec(request.Id);
        var costCenter = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

        if (costCenter is null)
            throw new NotFoundException($"{nameof(CostCenter)} with ID {request.Id} was not found.");

        return costCenter;
    }
}
