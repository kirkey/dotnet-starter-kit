using FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Delete.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Update.v1;

namespace Store.Infrastructure.Endpoints.v1;

public class WarehousesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("warehouses").WithTags("Warehouses");

        group.MapPost("/", async (CreateWarehouseCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/warehouses/{result.Id}", result);
        })
        .WithName("CreateWarehouse")
        .WithSummary("Create a new warehouse")
        .WithDescription("Creates a new warehouse with the provided details");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetWarehouse")
        .WithSummary("Get warehouse by ID")
        .WithDescription("Retrieves a warehouse by its unique identifier");

        group.MapGet("/", async (
            [AsParameters] GetWarehouseListQuery query, 
            ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetWarehouses")
        .WithSummary("Get list of warehouses")
        .WithDescription("Retrieves a paginated list of warehouses with optional filtering");

        group.MapPut("/{id:guid}", async (Guid id, UpdateWarehouseCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            await sender.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateWarehouse")
        .WithSummary("Update warehouse")
        .WithDescription("Updates an existing warehouse with the provided details");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteWarehouseCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteWarehouse")
        .WithSummary("Delete warehouse")
        .WithDescription("Deletes a warehouse by its unique identifier");
    }
}
