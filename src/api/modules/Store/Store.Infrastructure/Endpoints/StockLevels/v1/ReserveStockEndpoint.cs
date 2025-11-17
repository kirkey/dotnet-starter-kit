using FSH.Starter.WebApi.Store.Application.StockLevels.Reserve.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class ReserveStockEndpoint
{
    internal static RouteHandlerBuilder MapReserveStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reserve", async (DefaultIdType id, ReserveStockCommand request, ISender sender) =>
            {
                if (id != request.StockLevelId)
                {
                    return Results.BadRequest("ID in URL does not match StockLevelId in request body");
                }

                var response = await sender.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(ReserveStockEndpoint))
            .WithSummary("Reserve stock quantity")
            .WithDescription("Reserves quantity from available stock for orders or transfers (soft allocation)")
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .Produces<ReserveStockResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
