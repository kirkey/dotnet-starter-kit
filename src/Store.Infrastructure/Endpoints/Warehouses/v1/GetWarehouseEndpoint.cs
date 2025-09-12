using Store.Application.Warehouses.Get.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class GetWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapGetWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWarehouse")
        .WithSummary("Get warehouse by ID")
        .WithDescription("Retrieves a warehouse by its unique identifier")
        .MapToApiVersion(1);
    }
}
