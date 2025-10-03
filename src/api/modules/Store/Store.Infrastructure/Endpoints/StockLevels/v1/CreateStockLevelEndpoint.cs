using FSH.Starter.WebApi.Store.Application.StockLevels.Create.v1;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class CreateStockLevelEndpoint
{
    internal static RouteHandlerBuilder MapCreateStockLevelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateStockLevelCommand request, ISender sender) =>
            {
                var response = await sender.Send(request);
                return Results.Created($"/api/v1/store/stocklevels/{response.Id}", response);
            })
            .WithName(nameof(CreateStockLevelEndpoint))
            .WithSummary("Create a new stock level record")
            .WithDescription("Creates a new stock level tracking record for an item at a specific warehouse/location/bin")
            .RequirePermission("Permissions.Store.Create")
            .Produces<CreateStockLevelResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1);
    }
}
