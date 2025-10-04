using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

public static class SearchWarehouseLocationsEndpoint
{
    internal static RouteHandlerBuilder MapSearchWarehouseLocationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (ISender sender, [FromBody] SearchWarehouseLocationsCommand request) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(SearchWarehouseLocationsEndpoint))
        .WithSummary("Get list of warehouse locations")
        .WithDescription("Retrieves a paginated list of warehouse locations with optional filtering")
        .Produces<PagedList<GetWarehouseLocationListResponse>>()
        .MapToApiVersion(1);
    }
}
