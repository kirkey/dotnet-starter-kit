using FSH.Starter.WebApi.Store.Application.StockLevels.Allocate.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class AllocateStockEndpoint
{
    internal static RouteHandlerBuilder MapAllocateStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/allocate", async (DefaultIdType id, AllocateStockCommand request, ISender sender) =>
            {
                if (id != request.StockLevelId)
                {
                    return Results.BadRequest("ID in URL does not match StockLevelId in request body");
                }

                var response = await sender.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(AllocateStockEndpoint))
            .WithSummary("Allocate reserved stock")
            .WithDescription("Allocates reserved quantity to pick lists (hard allocation)")
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .Produces<AllocateStockResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
