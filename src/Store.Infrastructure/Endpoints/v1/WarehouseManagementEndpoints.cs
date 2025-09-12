using FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

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

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.TransferId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("ApproveInventoryTransfer")
        .WithSummary("Approve inventory transfer")
        .WithDescription("Approves an inventory transfer for shipping");

        group.MapPost("/{id:guid}/start-transit", async (Guid id, StartInventoryTransferTransitCommand command, ISender sender) =>
        {
            if (id != command.TransferId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("StartInventoryTransferTransit")
        .WithSummary("Start transfer transit")
        .WithDescription("Marks transfer as in transit with tracking information");

        group.MapPost("/{id:guid}/complete", async (Guid id, CompleteInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.TransferId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("CompleteInventoryTransfer")
        .WithSummary("Complete inventory transfer")
        .WithDescription("Completes the transfer and updates receiving warehouse inventory");

        group.MapPost("/search", async (SearchInventoryTransfersQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("SearchInventoryTransfers")
        .WithSummary("Search inventory transfers")
        .WithDescription("Search inventory transfers with filtering and pagination");
    }
}

public class StockAdjustmentsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("stock-adjustments").WithTags("Stock Adjustments");

        group.MapPost("/", async (CreateStockAdjustmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/stock-adjustments/{result.Id}", result);
        })
        .WithName("CreateStockAdjustment")
        .WithSummary("Create a new stock adjustment")
        .WithDescription("Creates a new stock adjustment for inventory corrections");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetStockAdjustmentQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetStockAdjustment")
        .WithSummary("Get stock adjustment by ID")
        .WithDescription("Retrieves a stock adjustment by its unique identifier");

        group.MapPost("/{id:guid}/approve", async (Guid id, ApproveStockAdjustmentCommand command, ISender sender) =>
        {
            if (id != command.AdjustmentId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("ApproveStockAdjustment")
        .WithSummary("Approve stock adjustment")
        .WithDescription("Approves a stock adjustment and applies changes to inventory");

        group.MapPost("/search", async (SearchStockAdjustmentsQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("SearchStockAdjustments")
        .WithSummary("Search stock adjustments")
        .WithDescription("Search stock adjustments with filtering and pagination");
    }
}
