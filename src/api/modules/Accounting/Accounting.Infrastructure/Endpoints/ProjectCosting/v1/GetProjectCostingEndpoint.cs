using Accounting.Application.ProjectCosting.Get.v1;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting.v1;

/// <summary>
/// Endpoint for retrieving a single project costing entry.
/// </summary>
public static class GetProjectCostingEndpoint
{
    /// <summary>
    /// Maps the get project costing endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGetProjectCostingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var query = new GetProjectCostingQuery(id);
            var response = await mediator.Send(query).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetProjectCostingEndpoint))
        .WithSummary("Get a project costing entry")
        .WithDescription("Retrieves a single project costing entry by ID")
        .Produces<ProjectCostingResponse>()
        .RequirePermission("Permissions.Accounting.View")
        .MapToApiVersion(1);
    }
}
