using FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class CreateWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapCreateWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateWarehouseCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/warehouses/{result.Id}", result);
        })
        .WithName("CreateWarehouse")
        .WithSummary("Create a new warehouse")
        .WithDescription("Creates a new warehouse with the provided details")
        .MapToApiVersion(1);
    }
}
