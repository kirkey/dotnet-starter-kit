using Accounting.Application.Projects.Delete;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectDeleteEndpoint
{
    internal static RouteHandlerBuilder MapProjectDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteProjectRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ProjectDeleteEndpoint))
            .WithSummary("delete project by id")
            .WithDescription("delete project by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}


