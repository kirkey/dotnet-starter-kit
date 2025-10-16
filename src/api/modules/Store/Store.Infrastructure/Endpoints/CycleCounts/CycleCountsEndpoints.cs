using Store.Infrastructure.Endpoints.CycleCounts.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts;

/// <summary>
/// Endpoint configuration for Cycle Counts module.
/// </summary>
public static class CycleCountsEndpoints
{
    /// <summary>
    /// Maps all Cycle Counts endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapCycleCountsEndpoints(this IEndpointRouteBuilder app)
    {
        var cycleCountsGroup = app.MapGroup("/cycle-counts")
            .WithTags("Cycle-Counts")
            .WithDescription("Endpoints for managing inventory cycle counts");

        // Version 1 endpoints
        cycleCountsGroup.MapCreateCycleCountEndpoint();
        cycleCountsGroup.MapGetCycleCountEndpoint();
        cycleCountsGroup.MapSearchCycleCountsEndpoint();
        cycleCountsGroup.MapStartCycleCountEndpoint();
        cycleCountsGroup.MapCompleteCycleCountEndpoint();
        cycleCountsGroup.MapCancelCycleCountEndpoint();
        cycleCountsGroup.MapReconcileCycleCountEndpoint();
        cycleCountsGroup.MapAddCycleCountItemEndpoint();
        cycleCountsGroup.MapRecordCycleCountItemEndpoint();

        return app;
    }
}
