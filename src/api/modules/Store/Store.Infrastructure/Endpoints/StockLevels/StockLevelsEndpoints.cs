using Carter;
using FSH.Starter.WebApi.Store.Application.StockLevels.Allocate.v1;
using FSH.Starter.WebApi.Store.Application.StockLevels.Create.v1;
using FSH.Starter.WebApi.Store.Application.StockLevels.Delete.v1;
using FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;
using FSH.Starter.WebApi.Store.Application.StockLevels.Release.v1;
using FSH.Starter.WebApi.Store.Application.StockLevels.Reserve.v1;
using FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1;
using FSH.Starter.WebApi.Store.Application.StockLevels.Update.v1;
using MediatR;
using GetStockLevelResponse = FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1.StockLevelResponse;
using SearchStockLevelResponse = FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1.StockLevelResponse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockLevels;

public class StockLevelsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/stock-levels").WithTags("stock-levels");

        // Create stock level
        group.MapPost("/", async (CreateStockLevelCommand request, ISender sender) =>
        {
            var response = await sender.Send(request);
            return Results.Created($"/api/v1/store/stock-levels/{response.Id}", response);
        })
        .WithName("CreateStockLevel")
        .WithSummary("Create a new stock level record")
        .WithDescription("Creates a new stock level tracking record for an item at a specific warehouse/location/bin")
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .Produces<CreateStockLevelResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1);

        // Update stock level
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateStockLevelCommand request, ISender sender) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID in URL does not match ID in request body");
            }

            var response = await sender.Send(request);
            return Results.Ok(response);
        })
        .WithName("UpdateStockLevel")
        .WithSummary("Update a stock level record")
        .WithDescription("Updates location/bin/lot/serial assignments for a stock level record")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .Produces<UpdateStockLevelResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);

        // Delete stock level
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteStockLevelCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteStockLevel")
        .WithSummary("Delete a stock level record")
        .WithDescription("Removes a stock level record (only allowed when quantity is zero)")
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);

        // Get stock level by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetStockLevelCommand(id));
            return Results.Ok(response);
        })
        .WithName("GetStockLevel")
        .WithSummary("Get a stock level by ID")
        .WithDescription("Retrieves detailed information about a specific stock level record")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .Produces<GetStockLevelResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);

        // Search stock levels
        group.MapPost("/search", async (SearchStockLevelsCommand command, ISender sender) =>
        {
            var response = await sender.Send(command);
            return Results.Ok(response);
        })
        .WithName("SearchStockLevels")
        .WithSummary("Search stock levels")
        .WithDescription("Search and filter stock levels with pagination support")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .Produces<PagedList<SearchStockLevelResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1);

        // Reserve stock
        group.MapPost("/{id:guid}/reserve", async (DefaultIdType id, ReserveStockCommand request, ISender sender) =>
        {
            if (id != request.StockLevelId)
            {
                return Results.BadRequest("ID in URL does not match StockLevelId in request body");
            }

            var response = await sender.Send(request);
            return Results.Ok(response);
        })
        .WithName("ReserveStock")
        .WithSummary("Reserve stock quantity")
        .WithDescription("Reserves quantity from available stock for orders or transfers (soft allocation)")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .Produces<ReserveStockResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);

        // Allocate stock
        group.MapPost("/{id:guid}/allocate", async (DefaultIdType id, AllocateStockCommand request, ISender sender) =>
        {
            if (id != request.StockLevelId)
            {
                return Results.BadRequest("ID in URL does not match StockLevelId in request body");
            }

            var response = await sender.Send(request);
            return Results.Ok(response);
        })
        .WithName("AllocateStock")
        .WithSummary("Allocate reserved stock")
        .WithDescription("Allocates reserved quantity to pick lists (hard allocation)")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .Produces<AllocateStockResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);

        // Release stock
        group.MapPost("/{id:guid}/release", async (DefaultIdType id, ReleaseStockCommand request, ISender sender) =>
        {
            if (id != request.StockLevelId)
            {
                return Results.BadRequest("ID in URL does not match StockLevelId in request body");
            }

            var response = await sender.Send(request);
            return Results.Ok(response);
        })
        .WithName("ReleaseStock")
        .WithSummary("Release reserved stock")
        .WithDescription("Releases reserved quantity back to available (e.g., when order is cancelled)")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .Produces<ReleaseStockResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);
    }
}
