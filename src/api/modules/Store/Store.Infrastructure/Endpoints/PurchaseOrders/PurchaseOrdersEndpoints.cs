using Carter;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Approve.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.AutoAddItems.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Cancel.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Create.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Delete.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Get.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.GetItemsNeedingReorder.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Add.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Get.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.ReceiveQuantity.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Remove.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdatePrice.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.UpdateQuantity.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Receive.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Search.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Send.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Submit.v1;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PurchaseOrders;

/// <summary>
/// Endpoint configuration for Purchase Orders module.
/// </summary>
public class PurchaseOrdersEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Purchase Orders endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/purchase-orders").WithTags("purchase-orders");

        // Create purchase order
        group.MapPost("/", async (CreatePurchaseOrderCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreatePurchaseOrderEndpoint")
        .WithSummary("Create a new purchase order")
        .WithDescription("Creates a new purchase order")
        .Produces<CreatePurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Update purchase order
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdatePurchaseOrderCommand command, ISender sender) =>
        {
            var updateCommand = command with { Id = id };
            var result = await sender.Send(updateCommand).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdatePurchaseOrderEndpoint")
        .WithSummary("Update a purchase order")
        .WithDescription("Updates an existing purchase order")
        .Produces<UpdatePurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Delete purchase order
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeletePurchaseOrderCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeletePurchaseOrderEndpoint")
        .WithSummary("Delete a purchase order")
        .WithDescription("Deletes a purchase order by ID")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        // Get purchase order
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetPurchaseOrderQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetPurchaseOrderEndpoint")
        .WithSummary("Get a purchase order")
        .WithDescription("Retrieves a purchase order by ID")
        .Produces<PurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Search purchase orders
        group.MapPost("/search", async (ISender mediator, [FromBody] SearchPurchaseOrdersCommand command) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchPurchaseOrdersEndpoint")
        .WithSummary("Search purchase orders")
        .WithDescription("Search and filter purchase orders with pagination support")
        .Produces<PagedList<PurchaseOrderResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Add purchase order item
        group.MapPost("/{id:guid}/items", async (DefaultIdType id, AddPurchaseOrderItemCommand request, ISender sender) =>
        {
            var command = request with { PurchaseOrderId = id };
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("AddPurchaseOrderItemEndpoint")
        .WithSummary("Add an item to a purchase order")
        .WithDescription("Adds a grocery item line to an existing purchase order. If the item already exists the aggregate will increase the quantity.")
        .Produces<AddPurchaseOrderItemResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Remove purchase order item
        group.MapDelete("/{id:guid}/items/{itemId:guid}", async (DefaultIdType id, DefaultIdType itemId, ISender sender) =>
        {
            var command = new RemovePurchaseOrderItemCommand(id, itemId);
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("RemovePurchaseOrderItemEndpoint")
        .WithSummary("Remove an item from a purchase order")
        .WithDescription("Removes an item line from a purchase order. Only allowed for modifiable orders.")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Update purchase order item quantity
        group.MapPut("/{id:guid}/items/{itemId:guid}/quantity", async (DefaultIdType id, DefaultIdType itemId, UpdatePurchaseOrderItemQuantityCommand request, ISender sender) =>
        {
            var command = request with { PurchaseOrderItemId = itemId };
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("UpdatePurchaseOrderItemQuantityEndpoint")
        .WithSummary("Update item quantity")
        .WithDescription("Updates the ordered quantity for a specific purchase order line item. Quantity cannot be less than already received quantity.")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Update purchase order item price
        group.MapPut("/{id:guid}/items/{itemId:guid}/price", async (DefaultIdType id, DefaultIdType itemId, UpdatePurchaseOrderItemPriceCommand request, ISender sender) =>
        {
            var command = request with { PurchaseOrderItemId = itemId };
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("UpdatePurchaseOrderItemPriceEndpoint")
        .WithSummary("Update item price and discount")
        .WithDescription("Updates the unit price and optional discount on a specific purchase order line item.")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Get purchase order items
        group.MapGet("/{id:guid}/items", async (DefaultIdType id, ISender sender) =>
        {
            var query = new GetPurchaseOrderItemsQuery(id);
            var response = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetPurchaseOrderItemsEndpoint")
        .WithSummary("Get all items for a purchase order")
        .WithDescription("Retrieves all line items associated with a purchase order, including grocery item details.")
        .Produces<List<PurchaseOrderItemResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Submit purchase order
        group.MapPost("/{id}/submit", async (DefaultIdType id, SubmitPurchaseOrderCommand command, ISender mediator) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Purchase order ID mismatch");
            }
            
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SubmitPurchaseOrderEndpoint")
        .WithSummary("Submit a purchase order for approval")
        .WithDescription("Submits a draft purchase order for approval, changing status from Draft to Submitted")
        .Produces<SubmitPurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Approve purchase order
        group.MapPost("/{id}/approve", async (DefaultIdType id, ApprovePurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new ApprovePurchaseOrderCommand(id, request.ApprovalNotes);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("ApprovePurchaseOrderEndpoint")
        .WithSummary("Approve a submitted purchase order")
        .WithDescription("Approves a submitted purchase order, changing status from Submitted to Approved")
        .Produces<ApprovePurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Store))
        .MapToApiVersion(1);

        // Send purchase order
        group.MapPost("/{id}/send", async (DefaultIdType id, SendPurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new SendPurchaseOrderCommand(id, request.DeliveryInstructions);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SendPurchaseOrderEndpoint")
        .WithSummary("Send an approved purchase order to supplier")
        .WithDescription("Sends an approved purchase order to the supplier, changing status from Approved to Sent")
        .Produces<SendPurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Send, FshResources.Store))
        .MapToApiVersion(1);

        // Receive purchase order
        group.MapPost("/{id}/receive", async (DefaultIdType id, ReceivePurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new ReceivePurchaseOrderCommand(id, request.ActualDeliveryDate, request.ReceiptNotes);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("ReceivePurchaseOrderEndpoint")
        .WithSummary("Receive a purchase order delivery")
        .WithDescription("Marks a sent purchase order as received and records the actual delivery date")
        .Produces<ReceivePurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Receive, FshResources.Store))
        .MapToApiVersion(1);

        // Receive purchase order item quantity
        group.MapPut("/{id:guid}/items/{itemId:guid}/receive", async (DefaultIdType id, DefaultIdType itemId, ReceivePurchaseOrderItemQuantityCommand request, ISender sender) =>
        {
            var command = request with { PurchaseOrderItemId = itemId };
            await sender.Send(command).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("ReceivePurchaseOrderItemQuantityEndpoint")
        .WithSummary("Record received quantity for an item")
        .WithDescription("Sets the received quantity for a purchase order line item (can be partial or complete).")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Cancel purchase order
        group.MapPost("/{id}/cancel", async (DefaultIdType id, CancelPurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new CancelPurchaseOrderCommand(id, request.CancellationReason);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CancelPurchaseOrderEndpoint")
        .WithSummary("Cancel a purchase order")
        .WithDescription("Cancels a purchase order that hasn't been received yet")
        .Produces<CancelPurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.Store))
        .MapToApiVersion(1);

        // Generate purchase order PDF
        group.MapGet("/{id}/pdf", async (DefaultIdType id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new GeneratePurchaseOrderPdfCommand(id);
            var pdfBytes = await sender.Send(command, cancellationToken);
            
            return Results.File(
                pdfBytes,
                "application/pdf",
                $"PurchaseOrder_{id}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf");
        })
        .WithName("GeneratePurchaseOrderPdfEndpoint")
        .WithSummary("Generate PDF report for a purchase order")
        .WithDescription("Generates a professional PDF report for the specified purchase order including all items and approval information.")
        .Produces<FileResult>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);

        // Auto-add items to purchase order
        group.MapPost("/{id:guid}/auto-add-items", async (DefaultIdType id, [FromBody] AutoAddItemsToPurchaseOrderCommand command, ISender sender) =>
        {
            var cmd = command with { PurchaseOrderId = id };
            var result = await sender.Send(cmd).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("AutoAddItemsToPurchaseOrderEndpoint")
        .WithSummary("Auto-add items needing reorder to purchase order")
        .WithDescription("Automatically adds items that are at or below their reorder point to the specified purchase order based on the order's supplier. Only works on Draft status purchase orders.")
        .Produces<AutoAddItemsToPurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Get items needing reorder
        group.MapPost("/suppliers/{supplierId:guid}/items-needing-reorder", async (DefaultIdType supplierId, [FromBody] GetItemsNeedingReorderRequest request, ISender sender) =>
        {
            var query = request with { SupplierId = supplierId };
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetItemsNeedingReorderEndpoint")
        .WithSummary("Get items needing reorder for a supplier")
        .WithDescription("Returns a list of items that are at or below their reorder point for the specified supplier, with suggested order quantities based on current stock levels")
        .Produces<List<ItemNeedingReorderResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}

/// <summary>
/// Request model for approve purchase order endpoint.
/// </summary>
public sealed record ApprovePurchaseOrderRequest(string? ApprovalNotes = null);

/// <summary>
/// Request model for send purchase order endpoint.
/// </summary>
public sealed record SendPurchaseOrderRequest(string? DeliveryInstructions = null);

/// <summary>
/// Request model for receive purchase order endpoint.
/// </summary>
public sealed record ReceivePurchaseOrderRequest(DateTime? ActualDeliveryDate = null, string? ReceiptNotes = null);

/// <summary>
/// Request model for cancel purchase order endpoint.
/// </summary>
public sealed record CancelPurchaseOrderRequest(string? CancellationReason = null);
