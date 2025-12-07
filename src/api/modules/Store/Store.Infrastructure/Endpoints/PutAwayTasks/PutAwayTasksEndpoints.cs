using Store.Infrastructure.Endpoints.PutAwayTasks.v1;
using Carter;

namespace Store.Infrastructure.Endpoints.PutAwayTasks;

/// <summary>
/// Endpoint configuration for Put-Away Tasks module.
/// Provides REST API endpoints for managing put-away tasks in warehouse operations.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class PutAwayTasksEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Put-Away Tasks endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, Search, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/put-away-tasks").WithTags("put-away-tasks");

        group.MapCreatePutAwayTaskEndpoint();
        group.MapAddPutAwayTaskItemEndpoint();
        group.MapAssignPutAwayTaskEndpoint();
        group.MapStartPutAwayEndpoint();
        group.MapCompletePutAwayEndpoint();
        group.MapDeletePutAwayTaskEndpoint();
        group.MapGetPutAwayTaskEndpoint();
        group.MapSearchPutAwayTasksEndpoint();
    }
}
