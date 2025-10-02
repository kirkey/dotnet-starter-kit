using Accounting.Application.ProjectCosting.Update.v1;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting.v1;

/// <summary>
/// Endpoint for updating an existing project costing entry.
/// </summary>
public static class UpdateProjectCostingEndpoint
{
    /// <summary>
    /// Maps the update project costing endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateProjectCostingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateProjectCostingCommand request, ISender mediator) =>
        {
            if (id != request.Id) return Results.BadRequest("ID mismatch");
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateProjectCostingEndpoint))
        .WithSummary("Update a project costing entry")
        .WithDescription("Updates an existing project costing entry")
        .Produces<UpdateProjectCostingResponse>()
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);
    }
}
