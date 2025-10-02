using Accounting.Infrastructure.Endpoints.ProjectCosting.v1;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting;

/// <summary>
/// Endpoint configuration for ProjectCosting module.
/// </summary>
public static class ProjectCostingEndpoints
{
    /// <summary>
    /// Maps all ProjectCosting endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapProjectCostingEndpoints(this IEndpointRouteBuilder app)
    {
        var projectCostingGroup = app.MapGroup("/projectcosting")
            .WithTags("ProjectCosting")
            .WithDescription("Endpoints for managing project costing entries");

        // Version 1 endpoints
        projectCostingGroup.MapCreateProjectCostingEndpoint();
        projectCostingGroup.MapUpdateProjectCostingEndpoint();
        projectCostingGroup.MapDeleteProjectCostingEndpoint();
        projectCostingGroup.MapGetProjectCostingEndpoint();
        projectCostingGroup.MapSearchProjectCostingEndpoint();

        return app;
    }
}
