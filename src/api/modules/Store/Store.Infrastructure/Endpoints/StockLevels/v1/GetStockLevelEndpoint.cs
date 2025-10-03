using FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class GetStockLevelEndpoint
{
    internal static RouteHandlerBuilder MapGetStockLevelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetStockLevelCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetStockLevelEndpoint))
            .WithSummary("Get a stock level by ID")
            .WithDescription("Retrieves detailed information about a specific stock level record")
            .RequirePermission("Permissions.Store.View")
            .Produces<StockLevelResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
