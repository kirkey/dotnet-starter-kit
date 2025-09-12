using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Delete.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Store.Infrastructure.Endpoints.v1;

public class InventoryTransfersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("inventory-transfers").WithTags("Inventory Transfers");

        group.MapPost("/", async (CreateInventoryTransferCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/inventory-transfers/{result.Id}", result);
        })
        .WithName("CreateInventoryTransfer")
        .WithSummary("Create a new inventory transfer")
        .WithDescription("Creates a new transfer between warehouses");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetInventoryTransferQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetInventoryTransfer")
        .WithSummary("Get inventory transfer by ID")
        .WithDescription("Retrieves an inventory transfer by its unique identifier");

        group.MapGet("/", async (
            [AsParameters] GetInventoryTransferListQuery query, 
            ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetInventoryTransfers")
        .WithSummary("Get list of inventory transfers")
        .WithDescription("Retrieves a paginated list of inventory transfers with optional filtering");

        group.MapPut("/{id:guid}", async (Guid id, UpdateInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            await sender.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateInventoryTransfer")
        .WithSummary("Update inventory transfer")
        .WithDescription("Updates an existing inventory transfer with the provided details");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteInventoryTransferCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteInventoryTransfer")
        .WithSummary("Delete inventory transfer")
        .WithDescription("Deletes an inventory transfer by its unique identifier");
    }
}
