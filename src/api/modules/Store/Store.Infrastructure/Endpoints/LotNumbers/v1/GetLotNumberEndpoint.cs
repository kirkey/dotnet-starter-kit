using FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1;

namespace Store.Infrastructure.Endpoints.LotNumbers.v1;

public static class GetLotNumberEndpoint
{
    internal static RouteHandlerBuilder MapGetLotNumberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetLotNumberCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetLotNumberEndpoint))
            .WithSummary("Get a lot number by ID")
            .WithDescription("Retrieves detailed information about a specific lot/batch number")
            .RequirePermission("Permissions.Store.View")
            .Produces<LotNumberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
