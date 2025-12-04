using Carter;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Create.v1;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Delete.v1;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Get.v1;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Search.v1;
using FSH.Starter.WebApi.Store.Application.WarehouseLocations.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.WarehouseLocations;

/// <summary>
/// Endpoint configuration for Warehouse Locations module.
/// </summary>
public class WarehouseLocationsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/warehouse-locations").WithTags("warehouse-locations");

        group.MapPost("/", async (CreateWarehouseLocationCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/warehouse-locations/{result.Id}", result);
        })
        .WithName("CreateWarehouseLocation")
        .WithSummary("Create a new warehouse location")
        .WithDescription("Creates a new warehouse location for storing items")
        .Produces<CreateWarehouseLocationResponse>()
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWarehouseLocationCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateWarehouseLocation")
        .WithSummary("Update warehouse location")
        .WithDescription("Updates an existing warehouse location with the provided details")
        .Produces<UpdateWarehouseLocationResponse>()
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteWarehouseLocationCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteWarehouseLocation")
        .WithSummary("Delete a warehouse location")
        .WithDescription("Deletes a warehouse location by ID")
        .Produces(204)
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetWarehouseLocationQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetWarehouseLocation")
        .WithSummary("Get warehouse location by ID")
        .WithDescription("Retrieves a warehouse location by its unique identifier")
        .Produces<GetWarehouseLocationResponse>()
        .MapToApiVersion(1);

        group.MapPost("/search", async (ISender sender, [FromBody] SearchWarehouseLocationsCommand request) =>
        {
            var result = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("SearchWarehouseLocations")
        .WithSummary("Get list of warehouse locations")
        .WithDescription("Retrieves a paginated list of warehouse locations with optional filtering")
        .Produces<PagedList<GetWarehouseLocationListResponse>>()
        .MapToApiVersion(1);
    }
}
