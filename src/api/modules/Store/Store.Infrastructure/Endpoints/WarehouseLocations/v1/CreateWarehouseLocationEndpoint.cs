using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Create.v1;

namespace Store.Infrastructure.Endpoints.WarehouseLocations.v1;

public static class CreateWarehouseLocationEndpoint
{
    internal static RouteHandlerBuilder MapCreateWarehouseLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateWarehouseLocationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/warehouse-locations/{result.Id}", result);
        })
        .WithName("CreateWarehouseLocation")
        .WithSummary("Create a new warehouse location")
        .WithDescription("Creates a new warehouse location for storing items")
        .Produces<CreateWarehouseLocationResponse>()
        .MapToApiVersion(1);
    }
}
