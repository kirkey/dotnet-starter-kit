namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class SearchWarehouseLocationsEndpoint
{
    internal static RouteHandlerBuilder MapSearchWarehouseLocationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/warehouse-locations", async ([AsParameters] FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1.GetWarehouseLocationListQuery query, ISender sender) =>
        {
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWarehouseLocations")
        .WithSummary("Get list of warehouse locations")
        .WithDescription("Retrieves a paginated list of warehouse locations with optional filtering")
        .MapToApiVersion(1);
    }
}

