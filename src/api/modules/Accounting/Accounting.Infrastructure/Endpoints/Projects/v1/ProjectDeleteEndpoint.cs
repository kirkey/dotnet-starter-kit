using Accounting.Application.Projects.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectDeleteEndpoint
{
    internal static RouteHandlerBuilder MapProjectDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteProjectCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectDeleteEndpoint))
            .WithSummary("Delete a project by id")
            .WithDescription("Deletes a project by its unique identifier and returns the result.")
            .Produces<DeleteProjectResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
