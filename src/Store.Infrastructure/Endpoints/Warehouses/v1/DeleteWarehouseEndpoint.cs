using FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Delete.v1;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class DeleteWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapDeleteWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteWarehouseCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteWarehouse")
        .WithSummary("Delete warehouse")
        .WithDescription("Deletes a warehouse by its unique identifier")
        .MapToApiVersion(1);
    }
}
using Store.Application.Warehouses.Create.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class CreateWarehouseEndpoint
{
    internal static RouteHandlerBuilder MapCreateWarehouseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/warehouses", async (CreateWarehouseCommand command, ISender sender) =>
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
