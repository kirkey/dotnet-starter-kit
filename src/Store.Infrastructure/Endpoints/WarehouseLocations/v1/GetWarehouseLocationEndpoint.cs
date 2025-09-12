using Store.Application.WarehouseLocations.Get.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

public static class GetWarehouseLocationEndpoint
{
    internal static RouteHandlerBuilder MapGetWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseLocationQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWarehouseLocation")
        .WithSummary("Get warehouse location by ID")
        .WithDescription("Retrieves a warehouse location by its unique identifier")
        .MapToApiVersion(1);
    }
}
