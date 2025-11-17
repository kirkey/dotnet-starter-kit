using FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1;
using Shared.Authorization;
using StockLevelResponse = FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1.StockLevelResponse;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class SearchStockLevelsEndpoint
{
    internal static RouteHandlerBuilder MapSearchStockLevelsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchStockLevelsCommand command, ISender sender) =>
            {
                var response = await sender.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchStockLevelsEndpoint))
            .WithSummary("Search stock levels")
            .WithDescription("Search and filter stock levels with pagination support")
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .Produces<PagedList<StockLevelResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1);
    }
}
