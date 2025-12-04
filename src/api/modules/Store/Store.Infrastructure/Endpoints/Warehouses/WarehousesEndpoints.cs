using Carter;
using FSH.Starter.WebApi.Store.Application.Warehouses.AssignManager.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Create.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Delete.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Get.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Search.v1;
using FSH.Starter.WebApi.Store.Application.Warehouses.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Warehouses;

/// <summary>
/// Endpoint configuration for Warehouses module.
/// </summary>
public class WarehousesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Warehouses endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var warehousesGroup = app.MapGroup("store/warehouses").WithTags("warehouses");

        warehousesGroup.MapPost("/", async (CreateWarehouseCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateWarehouseEndpoint")
        .WithSummary("Create a new warehouse")
        .WithDescription("Creates a new warehouse")
        .Produces<CreateWarehouseResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse))
        .MapToApiVersion(1);

        warehousesGroup.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateWarehouseEndpoint")
        .WithSummary("Update warehouse")
        .WithDescription("Updates an existing warehouse with the provided details")
        .Produces<UpdateWarehouseResponse>()
        .MapToApiVersion(1);

        warehousesGroup.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteWarehouseCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteWarehouseEndpoint")
        .WithSummary("Delete warehouse")
        .WithDescription("Deletes a warehouse by its unique identifier")
        .Produces(StatusCodes.Status204NoContent)
        .MapToApiVersion(1);

        warehousesGroup.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWarehouseEndpoint")
        .WithSummary("Get warehouse by ID")
        .WithDescription("Retrieves a warehouse by its unique identifier")
        .Produces<WarehouseResponse>()
        .MapToApiVersion(1);

        warehousesGroup.MapPost("/search", async (SearchWarehousesRequest request, ISender sender) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchWarehousesEndpoint")
        .WithSummary("Search warehouses")
        .WithDescription("Search and filter warehouses with pagination support")
        .Produces<PagedList<WarehouseResponse>>()
        .MapToApiVersion(1);

        warehousesGroup.MapPut("/{id:guid}/assign-manager", async (DefaultIdType id, AssignWarehouseManagerCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("AssignWarehouseManagerEndpoint")
        .WithSummary("Assign manager to warehouse")
        .WithDescription("Assigns a new manager to an existing warehouse")
        .Produces<AssignWarehouseManagerResponse>()
        .MapToApiVersion(1);
    }
}
