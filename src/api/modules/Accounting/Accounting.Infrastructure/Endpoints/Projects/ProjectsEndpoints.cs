using Accounting.Infrastructure.Endpoints.Projects.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Projects;

/// <summary>
/// Endpoint configuration for Projects module.
/// Provides comprehensive REST API endpoints for managing projects.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class ProjectsEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all Projects endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/projects").WithTags("project");

        group.MapProjectCreateEndpoint();
        group.MapProjectDeleteEndpoint();
        group.MapProjectGetEndpoint();
        group.MapProjectSearchEndpoint();
        group.MapProjectUpdateEndpoint();
    }
}
