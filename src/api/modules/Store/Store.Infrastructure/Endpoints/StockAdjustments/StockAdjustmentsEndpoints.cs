using Carter;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Approve.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Delete.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Search.v1;
using FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockAdjustments;

/// <summary>
/// Endpoint configuration for Stock Adjustments module.
/// </summary>
public class StockAdjustmentsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/stock-adjustments").WithTags("stock-adjustments");

        group.MapPost("/", async (CreateStockAdjustmentCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateStockAdjustmentEndpoint")
        .WithSummary("Create a new stock adjustment")
        .WithDescription("Creates a stock adjustment for inventory")
        .Produces<CreateStockAdjustmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateStockAdjustmentCommand command, ISender sender) =>
        {
            var updateCommand = command with { Id = id };
            var result = await sender.Send(updateCommand).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateStockAdjustmentEndpoint")
        .WithSummary("Update a stock adjustment")
        .WithDescription("Updates an existing stock adjustment")
        .Produces<UpdateStockAdjustmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteStockAdjustmentCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteStockAdjustmentEndpoint")
        .WithSummary("Delete a stock adjustment")
        .WithDescription("Deletes a stock adjustment by ID")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetStockAdjustmentQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetStockAdjustmentEndpoint")
        .WithSummary("Get stock adjustment by ID")
        .WithDescription("Retrieves a stock adjustment by its unique identifier")
        .Produces<StockAdjustmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        group.MapPost("/search", async (ISender sender, [FromBody] SearchStockAdjustmentsCommand request) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchStockAdjustmentsEndpoint")
        .WithSummary("Search stock adjustments")
        .WithDescription("Retrieves a paginated list of stock adjustments with optional filtering")
        .Produces<PagedList<StockAdjustmentResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveStockAdjustmentCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("ApproveStockAdjustmentEndpoint")
        .WithSummary("Approve stock adjustment")
        .WithDescription("Approves a stock adjustment and applies changes to inventory")
        .Produces<ApproveStockAdjustmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Store))
        .MapToApiVersion(1);
    }
}
