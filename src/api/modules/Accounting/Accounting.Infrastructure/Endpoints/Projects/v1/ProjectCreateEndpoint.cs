using Accounting.Application.Projects.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectCreateEndpoint
{
    internal static RouteHandlerBuilder MapProjectCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateProjectCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectCreateEndpoint))
            .WithSummary("create a project")
            .WithDescription("create a project")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}


