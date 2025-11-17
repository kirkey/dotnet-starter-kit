using FSH.Starter.WebApi.Store.Application.StockLevels.Update.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockLevels.v1;

public static class UpdateStockLevelEndpoint
{
    internal static RouteHandlerBuilder MapUpdateStockLevelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateStockLevelCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body");
                }

                var response = await sender.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateStockLevelEndpoint))
            .WithSummary("Update a stock level record")
            .WithDescription("Updates location/bin/lot/serial assignments for a stock level record")
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .Produces<UpdateStockLevelResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
