using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Create.v1;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Delete.v1;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Get.v1;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Update.v1;

namespace Store.Infrastructure.Endpoints.v1;

public class WarehouseLocationsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("warehouse-locations").WithTags("Warehouse Locations");

        group.MapPost("/", async (CreateWarehouseLocationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/warehouse-locations/{result.Id}", result);
        })
        .WithName("CreateWarehouseLocation")
        .WithSummary("Create a new warehouse location")
        .WithDescription("Creates a new storage location within a warehouse");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseLocationQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetWarehouseLocation")
        .WithSummary("Get warehouse location by ID")
        .WithDescription("Retrieves a warehouse location by its unique identifier");

        group.MapGet("/", async (
            [AsParameters] GetWarehouseLocationListQuery query, 
            ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetWarehouseLocations")
        .WithSummary("Get list of warehouse locations")
        .WithDescription("Retrieves a paginated list of warehouse locations with optional filtering");

        group.MapPut("/{id:guid}", async (Guid id, UpdateWarehouseLocationCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            await sender.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateWarehouseLocation")
        .WithSummary("Update warehouse location")
        .WithDescription("Updates an existing warehouse location with the provided details");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteWarehouseLocationCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteWarehouseLocation")
        .WithSummary("Delete warehouse location")
        .WithDescription("Deletes a warehouse location by its unique identifier");
    }
}
