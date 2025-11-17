using Accounting.Application.CostCenters.Responses;
using Accounting.Application.CostCenters.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterSearchEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchCostCentersRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CostCenterSearchEndpoint))
            .WithSummary("Search cost centers")
            .Produces<List<CostCenterResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


