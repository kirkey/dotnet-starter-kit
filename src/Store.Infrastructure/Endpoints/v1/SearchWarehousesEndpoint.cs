namespace Store.Infrastructure.Endpoints.v1;

public static class SearchWarehousesEndpoint
{
    internal static RouteHandlerBuilder MapSearchWarehousesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/warehouses", async ([AsParameters] FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1.GetWarehouseListQuery query, ISender sender) =>
        {
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWarehouses")
        .WithSummary("Get list of warehouses")
        .WithDescription("Retrieves a paginated list of warehouses with optional filtering")
        .MapToApiVersion(1);
    }
}

