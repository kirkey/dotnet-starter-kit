using Accounting.Infrastructure.Endpoints.Projects.v1;

namespace Accounting.Infrastructure.Endpoints.Projects;

/// <summary>
/// Endpoint configuration for Projects module.
/// </summary>
public static class ProjectsEndpoints
{
    /// <summary>
    /// Maps all Projects endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        var projectsGroup = app.MapGroup("/projects")
            .WithTags("Projects")
            .WithDescription("Endpoints for managing projects");

        // Version 1 endpoints
        projectsGroup.MapProjectCreateEndpoint();
        projectsGroup.MapProjectUpdateEndpoint();
        projectsGroup.MapProjectDeleteEndpoint();
        projectsGroup.MapProjectGetEndpoint();
        projectsGroup.MapProjectSearchEndpoint();

        return app;
    }
}
