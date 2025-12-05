using Store.Infrastructure.Endpoints.CycleCounts.v1;
using Carter;

namespace Store.Infrastructure.Endpoints.CycleCounts;

/// <summary>
/// Endpoint configuration for Cycle Counts module.
/// Provides REST API endpoints for managing cycle counts and inventory reconciliation.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class CycleCountsEndpoints() : CarterModule("store")
{
    /// <summary>
    /// Maps all Cycle Counts endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, Search, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/cycle-counts").WithTags("cycle-counts");

        group.MapCreateCycleCountEndpoint();
        group.MapGetCycleCountEndpoint();
        group.MapUpdateCycleCountEndpoint();
        group.MapSearchCycleCountsEndpoint();
        group.MapStartCycleCountEndpoint();
        group.MapCompleteCycleCountEndpoint();
        group.MapCancelCycleCountEndpoint();
        group.MapReconcileCycleCountEndpoint();
        group.MapAddCycleCountItemEndpoint();
        group.MapRecordCycleCountItemEndpoint();
        group.MapSearchCycleCountItemsEndpoint();
        group.MapUpdateCycleCountItemEndpoint();
    }
}
