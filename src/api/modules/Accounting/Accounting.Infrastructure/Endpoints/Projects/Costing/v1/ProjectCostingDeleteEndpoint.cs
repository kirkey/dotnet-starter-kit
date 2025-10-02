using Accounting.Application.Projects.Costing.Delete;

namespace Accounting.Infrastructure.Endpoints.Projects.Costing.v1;

public static class ProjectCostingDeleteEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostingDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteProjectCostingCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(ProjectCostingDeleteEndpoint))
            .WithSummary("delete project costing entry")
            .WithDescription("Deletes a project costing entry")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
