using Store.Application.WarehouseLocations.Search.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

public static class SearchWarehouseLocationsEndpoint
{
    internal static RouteHandlerBuilder MapSearchWarehouseLocationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async ([AsParameters] SearchWarehouseLocationsQuery query, ISender sender) =>
        {
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchWarehouseLocations")
        .WithSummary("Get list of warehouse locations")
        .WithDescription("Retrieves a paginated list of warehouse locations with optional filtering")
        .MapToApiVersion(1);
    }
}
using Store.Application.WarehouseLocations.Create.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

public static class CreateWarehouseLocationEndpoint
{
    internal static RouteHandlerBuilder MapCreateWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/warehouse-locations", async (CreateWarehouseLocationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/warehouse-locations/{result.Id}", result);
        })
        .WithName("CreateWarehouseLocation")
        .WithSummary("Create a new warehouse location")
        .WithDescription("Creates a new warehouse location for storing items")
        .MapToApiVersion(1);
    }
}
