using Accounting.Application.ProjectCosting.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting.v1;

/// <summary>
/// Endpoint for deleting a project costing entry.
/// </summary>
public static class DeleteProjectCostingEndpoint
{
    /// <summary>
    /// Maps the delete project costing endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapDeleteProjectCostingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeleteProjectCostingCommand(id);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(DeleteProjectCostingEndpoint))
        .WithSummary("Delete a project costing entry")
        .WithDescription("Deletes a project costing entry")
        .Produces<DeleteProjectCostingResponse>()
        .RequirePermission("Permissions.Accounting.Delete")
        .MapToApiVersion(1);
    }
}
