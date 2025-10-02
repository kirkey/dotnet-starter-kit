namespace Accounting.Application.Projects.Costing.Get.v1;

/// <summary>
/// Handler for retrieving project cost entry details.
/// </summary>
public sealed class GetProjectCostHandler(
    ILogger<GetProjectCostHandler> logger,
    [FromKeyedServices("accounting:projectcosts")] IRepository<ProjectCostEntry> repository)
    : IRequestHandler<GetProjectCostQuery, GetProjectCostResponse>
{
    public async Task<GetProjectCostResponse> Handle(GetProjectCostQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cost = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProjectCostNotFoundException(request.Id);

        logger.LogInformation("project cost retrieved {ProjectCostId}", cost.Id);

        return cost.Adapt<GetProjectCostResponse>();
    }
}
