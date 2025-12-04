using Carter;
using FSH.Starter.WebApi.Store.Application.SerialNumbers.Create.v1;
using FSH.Starter.WebApi.Store.Application.SerialNumbers.Delete.v1;
using FSH.Starter.WebApi.Store.Application.SerialNumbers.Get.v1;
using FSH.Starter.WebApi.Store.Application.SerialNumbers.Search.v1;
using FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.SerialNumbers;

/// <summary>
/// Endpoint configuration for Serial Numbers module.
/// </summary>
public class SerialNumbersEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Serial Numbers endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/serial-numbers").WithTags("serial-numbers");

        // Create serial number
        group.MapPost("/", async (CreateSerialNumberCommand request, ISender sender) =>
        {
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateSerialNumber")
        .WithSummary("Create a new serial number")
        .WithDescription("Creates a new serial number for unit-level tracking of inventory items.")
        .Produces<CreateSerialNumberResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Update serial number
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateSerialNumberCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateSerialNumber")
        .WithSummary("Update a serial number")
        .WithDescription("Updates an existing serial number's status, location, or other properties.")
        .Produces<UpdateSerialNumberResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Delete serial number
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new DeleteSerialNumberCommand { Id = id }).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeleteSerialNumber")
        .WithSummary("Delete a serial number")
        .WithDescription("Deletes an existing serial number from the system.")
        .Produces<DeleteSerialNumberResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        // Get serial number by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetSerialNumberCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetSerialNumber")
        .WithSummary("Get a serial number by ID")
        .WithDescription("Retrieves a specific serial number by its unique identifier.")
        .Produces<SerialNumberResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Search serial numbers
        group.MapPost("/search", async (SearchSerialNumbersCommand command, ISender sender) =>
        {
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchSerialNumbers")
        .WithSummary("Search serial numbers")
        .WithDescription("Searches for serial numbers with pagination and filtering by serial value, item, warehouse, status, receipt date, warranty status, and external reference.")
        .Produces<PagedList<SerialNumberResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}
