using FSH.Starter.WebApi.Store.Application.StockLevels.Delete.v1;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class DeleteStockLevelEndpoint
{
    internal static RouteHandlerBuilder MapDeleteStockLevelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                await sender.Send(new DeleteStockLevelCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteStockLevelEndpoint))
            .WithSummary("Delete a stock level record")
            .WithDescription("Removes a stock level record (only allowed when quantity is zero)")
            .RequirePermission("Permissions.Store.Delete")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
