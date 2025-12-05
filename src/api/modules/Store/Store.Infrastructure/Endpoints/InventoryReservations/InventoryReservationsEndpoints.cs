using FSH.Starter.WebApi.Store.Application.InventoryReservations.Create.v1;
using FSH.Starter.WebApi.Store.Application.InventoryReservations.Delete.v1;
using FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1;
using FSH.Starter.WebApi.Store.Application.InventoryReservations.Release.v1;
using FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;
using Shared.Authorization;
using GetInventoryReservationResponse = FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1.InventoryReservationResponse;

namespace Store.Infrastructure.Endpoints.InventoryReservations;

/// <summary>
/// Endpoint configuration for Inventory Reservations module.
/// </summary>
public class InventoryReservationsEndpoints() : CarterModule("store")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/inventory-reservations").WithTags("inventory-reservations");

        // Create inventory reservation
        group.MapPost("/", async (CreateInventoryReservationCommand request, ISender sender) =>
        {
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateInventoryReservationEndpoint")
        .WithSummary("Create a new inventory reservation")
        .WithDescription("Creates a new inventory reservation to prevent overselling and support order fulfillment.")
        .Produces<CreateInventoryReservationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Get inventory reservation by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetInventoryReservationCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetInventoryReservationEndpoint")
        .WithSummary("Get an inventory reservation by ID")
        .WithDescription("Retrieves a specific inventory reservation by its unique identifier.")
        .Produces<GetInventoryReservationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Release inventory reservation
        group.MapPost("/{id:guid}/release", async (DefaultIdType id, ReleaseInventoryReservationCommand request, ISender sender) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID mismatch between route and body.");
            }

            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("ReleaseInventoryReservationEndpoint")
        .WithSummary("Release an inventory reservation")
        .WithDescription("Releases an active inventory reservation, returning the quantity to available stock.")
        .Produces<ReleaseInventoryReservationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Search inventory reservations
        group.MapPost("/search", async (SearchInventoryReservationsCommand request, ISender sender) =>
        {
            var response = await sender.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchInventoryReservationsEndpoint")
        .WithSummary("Search inventory reservations")
        .WithDescription("Searches for inventory reservations with pagination and filtering by reservation number, item, warehouse, type, status, dates, and more.")
        .Produces<PagedList<InventoryReservationDto>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Delete inventory reservation
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new DeleteInventoryReservationCommand { Id = id }).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeleteInventoryReservationEndpoint")
        .WithSummary("Delete an inventory reservation")
        .WithDescription("Deletes an existing inventory reservation from the system.")
        .Produces<DeleteInventoryReservationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);
    }
}
