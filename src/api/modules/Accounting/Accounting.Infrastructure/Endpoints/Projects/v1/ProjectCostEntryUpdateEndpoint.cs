using Accounting.Application.Projects.Costs.Update;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectCostEntryUpdateEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostEntryUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{projectId:guid}/costs/{entryId:guid}", async (DefaultIdType projectId, DefaultIdType entryId, UpdateProjectCostEntryCommand body, ISender mediator) =>
            {
                if (projectId != body.ProjectId || entryId != body.EntryId) return Results.BadRequest();
                var id = await mediator.Send(body).ConfigureAwait(false);
                return Results.Ok(id);
            })
            .WithName(nameof(ProjectCostEntryUpdateEndpoint))
            .WithSummary("update a project cost entry")
            .WithDescription("update a project cost entry by id for a project")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
