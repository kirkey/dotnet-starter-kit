using Accounting.Infrastructure.Endpoints.Projects.Costing.v1;

namespace Accounting.Infrastructure.Endpoints.Projects.Costing;

/// <summary>
/// Endpoint configuration for ProjectCosting module under Projects.
/// </summary>
public static class ProjectsCostingEndpoints
{
    /// <summary>
    /// Maps all Projects Costing endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapProjectsCostingEndpoints(this IEndpointRouteBuilder app)
    {
        var costingGroup = app.MapGroup("/projects/costing")
            .WithTags("Projects-Costing")
            .WithDescription("Endpoints for managing project costing entries under projects");

        // Version 1 endpoints
        costingGroup.MapProjectCostingCreateEndpoint();
        costingGroup.MapProjectCostingUpdateEndpoint();
        costingGroup.MapProjectCostingDeleteEndpoint();
        costingGroup.MapProjectCostingGetEndpoint();
        costingGroup.MapProjectCostingSearchEndpoint();

        return app;
    }
}
