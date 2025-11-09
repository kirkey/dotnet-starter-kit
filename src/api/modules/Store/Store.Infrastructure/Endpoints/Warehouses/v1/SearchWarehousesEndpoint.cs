using FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

/// <summary>
/// Endpoint for searching warehouses with pagination and filtering capabilities.
/// </summary>
public static class SearchWarehousesEndpoint
{
    /// <summary>
    /// Maps the search warehouses endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for search warehouses endpoint</returns>
    internal static RouteHandlerBuilder MapSearchWarehousesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (SearchWarehousesRequest request, ISender sender) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(SearchWarehousesEndpoint))
        .WithSummary("Search warehouses")
        .WithDescription("Search and filter warehouses with pagination support")
        .Produces<PagedList<WarehouseResponse>>()
        .MapToApiVersion(1);
    }
}
