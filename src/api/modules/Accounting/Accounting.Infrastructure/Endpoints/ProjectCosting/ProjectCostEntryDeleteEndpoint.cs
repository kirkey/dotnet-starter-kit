using Accounting.Application.ProjectCosting.Delete;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting;

public static class ProjectCostEntryDeleteEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostEntryDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{projectId:guid}/costs/{entryId:guid}", async (DefaultIdType projectId, DefaultIdType entryId, ISender mediator) =>
            {
                await mediator.Send(new DeleteProjectCostEntryCommand(projectId, entryId)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ProjectCostEntryDeleteEndpoint))
            .WithSummary("delete a project cost entry")
            .WithDescription("delete a project cost entry by id for a project")
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}
