using Carter;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Approve.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Cancel.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Complete.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Delete.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Remove.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Update.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.MarkInTransit.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;
using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransfers;

/// <summary>
/// Endpoint configuration for Inventory Transfers module.
/// </summary>
public class InventoryTransfersEndpoints : ICarterModule
{
    /// <summary>
    /// Adds all Inventory Transfers routes to the application.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/inventory-transfers").WithTags("inventory-transfers");

        // Create inventory transfer
        group.MapPost("/", async (CreateInventoryTransferCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateInventoryTransfer")
        .WithSummary("Create a new inventory transfer")
        .WithDescription("Creates a new transfer between warehouses")
        .Produces<CreateInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Update inventory transfer
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateInventoryTransferCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateInventoryTransfer")
        .WithSummary("Update inventory transfer")
        .WithDescription("Updates an existing inventory transfer with the provided details")
        .Produces<UpdateInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Delete inventory transfer
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteInventoryTransferCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteInventoryTransfer")
        .WithSummary("Delete an inventory transfer")
        .WithDescription("Deletes an inventory transfer by ID")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Get inventory transfer by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetInventoryTransferQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetInventoryTransfer")
        .WithSummary("Get inventory transfer by ID")
        .WithDescription("Retrieves an inventory transfer by its unique identifier")
        .Produces<GetInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Search inventory transfers
        group.MapPost("/search", async (ISender sender, [Microsoft.AspNetCore.Mvc.FromBody] SearchInventoryTransfersCommand request) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchInventoryTransfers")
        .WithSummary("Get list of inventory transfers")
        .WithDescription("Retrieves a paginated list of inventory transfers with optional filtering")
        .Produces<PagedList<GetInventoryTransferListResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Add item to inventory transfer
        group.MapPost("/{id:guid}/items", async (DefaultIdType id, AddInventoryTransferItemCommand command, ISender sender) =>
        {
            if (id != command.InventoryTransferId) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("AddInventoryTransferItem")
        .WithSummary("Add item to inventory transfer")
        .WithDescription("Adds a grocery item line to an existing inventory transfer")
        .Produces<AddInventoryTransferItemResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Remove item from inventory transfer
        group.MapDelete("/{id:guid}/items/{itemId:guid}", async (DefaultIdType id, DefaultIdType itemId, ISender sender) =>
        {
            await sender.Send(new RemoveInventoryTransferItemCommand(id, itemId)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("RemoveInventoryTransferItem")
        .WithSummary("Remove item from inventory transfer")
        .WithDescription("Removes a grocery item line from an existing inventory transfer")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Update inventory transfer item
        group.MapPut("/{id:guid}/items/{itemId:guid}", async (DefaultIdType id, DefaultIdType itemId, UpdateInventoryTransferItemCommand request, ISender sender) =>
        {
            var command = request with { InventoryTransferId = id, ItemId = itemId };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateInventoryTransferItem")
        .WithSummary("Update inventory transfer item")
        .WithDescription("Updates quantity and unit price of an item within an existing inventory transfer")
        .Produces<UpdateInventoryTransferItemResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Approve inventory transfer
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("ApproveInventoryTransfer")
        .WithSummary("Approve inventory transfer")
        .WithDescription("Approves an inventory transfer")
        .Produces<ApproveInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Mark inventory transfer as in-transit
        group.MapPost("/{id:guid}/in-transit", async (DefaultIdType id, MarkInTransitInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("MarkInTransitInventoryTransfer")
        .WithSummary("Mark inventory transfer as in-transit")
        .WithDescription("Marks an approved inventory transfer as InTransit")
        .Produces<MarkInTransitInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Complete inventory transfer
        group.MapPost("/{id:guid}/complete", async (DefaultIdType id, CompleteInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("CompleteInventoryTransfer")
        .WithSummary("Complete inventory transfer")
        .WithDescription("Marks an in-transit inventory transfer as completed and records actual arrival")
        .Produces<CompleteInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);

        // Cancel inventory transfer
        group.MapPost("/{id:guid}/cancel", async (DefaultIdType id, CancelInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("CancelInventoryTransfer")
        .WithSummary("Cancel inventory transfer")
        .WithDescription("Cancels a pending or approved inventory transfer with optional reason")
        .Produces<CancelInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.Warehouse))
        .MapToApiVersion(1);
    }
}
