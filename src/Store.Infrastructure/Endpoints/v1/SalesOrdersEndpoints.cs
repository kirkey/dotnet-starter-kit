// Auto-generated: Sales Orders endpoints (Catalog-style static mapping)

namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class SalesOrdersEndpoints
{
    public static void MapSalesOrdersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // This file has been split into Catalog-style single-endpoint files:
        // - CreateSalesOrderEndpoint.cs
        // - GetSalesOrderEndpoint.cs
        // - UpdateSalesOrderEndpoint.cs
        // - AddSalesOrderItemEndpoint.cs
        // The grouped endpoints are now removed for consistency
    }
}
// Auto-generated: Stock Adjustments endpoints (Catalog-style static mapping)
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace FSH.Starter.WebApi.Store.Infrastructure.Endpoints.v1;

public static class StockAdjustmentsEndpoints
{
    public static void MapStockAdjustmentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var adjustments = endpoints.MapGroup("stock-adjustments").WithTags("Stock Adjustments");

        adjustments.MapPost("/", async (FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1.CreateStockAdjustmentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/stock-adjustments/{result.Id}", result);
        })
        .WithName("CreateStockAdjustment")
        .WithSummary("Create a new stock adjustment")
        .WithDescription("Creates a new stock adjustment for inventory corrections")
        .MapToApiVersion(1);

        adjustments.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1.GetStockAdjustmentQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetStockAdjustment")
        .WithSummary("Get stock adjustment by ID")
        .WithDescription("Retrieves a stock adjustment by its unique identifier")
        .MapToApiVersion(1);

        adjustments.MapPost("/{id:guid}/approve", async (Guid id, FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1.ApproveStockAdjustmentCommand command, ISender sender) =>
        {
            if (id != command.AdjustmentId) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("ApproveStockAdjustment")
        .WithSummary("Approve stock adjustment")
        .WithDescription("Approves a stock adjustment and applies changes to inventory")
        .MapToApiVersion(1);

        adjustments.MapPost("/search", async (FSH.Starter.WebApi.Store.Application.StockAdjustments.Search.v1.SearchStockAdjustmentsQuery query, ISender sender) =>
        {
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchStockAdjustments")
        .WithSummary("Search stock adjustments")
        .WithDescription("Search stock adjustments with filtering and pagination")
        .MapToApiVersion(1);
    }
}
