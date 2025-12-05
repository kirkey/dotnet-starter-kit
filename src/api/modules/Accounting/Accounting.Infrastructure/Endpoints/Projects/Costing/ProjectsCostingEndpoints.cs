using Accounting.Infrastructure.Endpoints.Projects.Costing.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Projects.Costing;

/// <summary>
/// Carter module for ProjectCosting endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class ProjectsCostingEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all Projects Costing endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/projects/costing").WithTags("projects-costing");

        // CRUD operations
        group.MapProjectCostingCreateEndpoint();
        group.MapProjectCostingUpdateEndpoint();
        group.MapProjectCostingDeleteEndpoint();
        group.MapProjectCostingGetEndpoint();
        group.MapProjectCostingSearchEndpoint();
    }
}
