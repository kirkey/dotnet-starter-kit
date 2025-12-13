using FSH.Starter.WebApi.Store.Application.StockLevels.Release.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class ReleaseStockEndpoint
{
    internal static RouteHandlerBuilder MapReleaseStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/release", async (DefaultIdType id, ReleaseStockCommand request, ISender sender) =>
            {
                if (id != request.StockLevelId)
                {
                    return Results.BadRequest("ID in URL does not match StockLevelId in request body");
                }

                var response = await sender.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(ReleaseStockEndpoint))
            .WithSummary("Release reserved stock")
            .WithDescription("Releases reserved quantity back to available (e.g., when order is cancelled)")
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .Produces<ReleaseStockResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
