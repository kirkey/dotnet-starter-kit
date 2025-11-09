using FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

/// <summary>
/// Endpoint for getting a warehouse by ID.
/// </summary>
public static class GetWarehouseEndpoint
{
    /// <summary>
    /// Maps the get warehouse endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapGetWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetWarehouseEndpoint))
        .WithSummary("Get warehouse by ID")
        .WithDescription("Retrieves a warehouse by its unique identifier")
        .Produces<WarehouseResponse>()
        .MapToApiVersion(1);
    }
}
