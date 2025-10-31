using Accounting.Application.CostCenters.Create.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterCreateEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CostCenterCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/cost-centers/{response.Id}", response);
            })
            .WithName(nameof(CostCenterCreateEndpoint))
            .WithSummary("Create cost center")
            .Produces<CostCenterCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

