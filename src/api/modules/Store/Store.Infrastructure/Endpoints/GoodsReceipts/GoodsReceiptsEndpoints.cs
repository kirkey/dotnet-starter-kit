using FSH.Starter.WebApi.Store.Application.GoodsReceipts.AddItem.v1;
using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;
using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Delete.v1;
using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Get.v1;
using FSH.Starter.WebApi.Store.Application.GoodsReceipts.MarkReceived.v1;
using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Queries;
using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Search.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.GoodsReceipts;

/// <summary>
/// Endpoint configuration for Goods Receipts module.
/// </summary>
public class GoodsReceiptsEndpoints() : CarterModule
{

    private const string AddGoodsReceiptItemEndpoint = "AddGoodsReceiptItemEndpoint";
    private const string CreateGoodsReceiptEndpoint = "CreateGoodsReceiptEndpoint";
    private const string DeleteGoodsReceiptEndpoint = "DeleteGoodsReceiptEndpoint";
    private const string GetGoodsReceiptEndpoint = "GetGoodsReceiptEndpoint";
    private const string GetPurchaseOrderItemsForReceivingEndpoint = "GetPurchaseOrderItemsForReceivingEndpoint";
    private const string MarkReceivedEndpoint = "MarkReceivedEndpoint";
    private const string SearchGoodsReceiptsEndpoint = "SearchGoodsReceiptsEndpoint";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/goods-receipts").WithTags("goods-receipts");

        // Create a new goods receipt
        group.MapPost("/", async (CreateGoodsReceiptCommand request, ISender sender) =>
        {
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(CreateGoodsReceiptEndpoint)
        .WithSummary("Create a new goods receipt")
        .WithDescription("Creates a new goods receipt for tracking inbound deliveries from suppliers.")
        .Produces<CreateGoodsReceiptResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Add item to goods receipt
        group.MapPost("/{id}/items", async (DefaultIdType id, AddGoodsReceiptItemCommand request, ISender sender) =>
        {
            if (id != request.GoodsReceiptId)
            {
                return Results.BadRequest("Goods receipt ID mismatch");
            }

            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(AddGoodsReceiptItemEndpoint)
        .WithSummary("Add item to goods receipt")
        .WithDescription("Adds an item to an existing goods receipt.")
        .Produces<AddGoodsReceiptItemResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Mark goods receipt as received
        group.MapPost("/{id}/mark-received", async (DefaultIdType id, MarkReceivedCommand request, ISender sender) =>
        {
            if (id != request.GoodsReceiptId)
            {
                return Results.BadRequest("Goods receipt ID mismatch");
            }

            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(MarkReceivedEndpoint)
        .WithSummary("Mark goods receipt as received")
        .WithDescription("Marks a goods receipt as received/completed.")
        .Produces<MarkReceivedResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Delete a goods receipt
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var request = new DeleteGoodsReceiptCommand { GoodsReceiptId = id };
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(DeleteGoodsReceiptEndpoint)
        .WithSummary("Delete a goods receipt")
        .WithDescription("Deletes an existing goods receipt.")
        .Produces<DeleteGoodsReceiptResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        // Get goods receipt by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var command = new GetGoodsReceiptCommand { GoodsReceiptId = id };
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(GetGoodsReceiptEndpoint)
        .WithSummary("Get goods receipt by ID")
        .WithDescription("Retrieves a specific goods receipt with all items.")
        .Produces<GetGoodsReceiptResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Search goods receipts
        group.MapPost("/search", async (SearchGoodsReceiptsCommand request, ISender sender) =>
        {
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(SearchGoodsReceiptsEndpoint)
        .WithSummary("Search goods receipts")
        .WithDescription("Searches goods receipts with pagination and filtering options.")
        .Produces<PagedList<GoodsReceiptResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Get PO items available for receiving
        group.MapGet("/purchase-order/{purchaseOrderId:guid}/items-for-receiving", async (
            DefaultIdType purchaseOrderId,
            ISender sender) =>
        {
            var query = new GetPurchaseOrderItemsForReceivingQuery
            {
                PurchaseOrderId = purchaseOrderId
            };

            var response = await sender.Send(query);
            return Results.Ok(response);
        })
        .WithName(GetPurchaseOrderItemsForReceivingEndpoint)
        .WithSummary("Get PO items available for receiving")
        .WithDescription("Returns purchase order items with their ordered, received, and remaining quantities for partial receiving support")
        .Produces<GetPurchaseOrderItemsForReceivingResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}
