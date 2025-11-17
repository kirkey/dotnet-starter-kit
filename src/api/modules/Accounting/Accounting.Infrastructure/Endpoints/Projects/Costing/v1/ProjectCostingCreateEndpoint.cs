using Accounting.Application.Projects.Costing.Create;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Projects.Costing.v1;

public static class ProjectCostingCreateEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostingCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost(string.Empty, async (CreateProjectCostingCommand request, ISender mediator) =>
            {
                var id = await mediator.Send(request);
                return Results.Ok(id);
            })
            .WithName(nameof(ProjectCostingCreateEndpoint))
            .WithSummary("create project costing entry")
            .WithDescription("Creates a new project costing entry for tracking project expenses")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
