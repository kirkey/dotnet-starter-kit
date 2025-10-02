using Accounting.Application.ProjectCosting.Create.v1;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting.v1;

/// <summary>
/// Endpoint for creating a new project costing entry.
/// </summary>
public static class CreateProjectCostingEndpoint
{
    /// <summary>
    /// Maps the create project costing endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapCreateProjectCostingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateProjectCostingCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreateProjectCostingEndpoint))
        .WithSummary("Create a new project costing entry")
        .WithDescription("Creates a new project costing entry for a project")
        .Produces<CreateProjectCostingResponse>()
        .RequirePermission("Permissions.Accounting.Create")
        .MapToApiVersion(1);
    }
}
