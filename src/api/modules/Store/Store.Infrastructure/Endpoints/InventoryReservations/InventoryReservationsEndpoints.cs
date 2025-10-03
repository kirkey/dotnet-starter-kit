using Store.Infrastructure.Endpoints.InventoryReservations.v1;

namespace Store.Infrastructure.Endpoints.InventoryReservations;

/// <summary>
/// Endpoint configuration for Inventory Reservations module.
/// </summary>
public static class InventoryReservationsEndpoints
{
    /// <summary>
    /// Maps all Inventory Reservations endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapInventoryReservationsEndpoints(this IEndpointRouteBuilder app)
    {
        var reservationsGroup = app.MapGroup("/inventoryreservations")
            .WithTags("InventoryReservations")
            .WithDescription("Endpoints for managing inventory reservations");

        // Version 1 endpoints
        reservationsGroup.MapCreateInventoryReservationEndpoint();
        reservationsGroup.MapReleaseInventoryReservationEndpoint();
        reservationsGroup.MapDeleteInventoryReservationEndpoint();
        reservationsGroup.MapGetInventoryReservationEndpoint();
        reservationsGroup.MapSearchInventoryReservationsEndpoint();

        return app;
    }
}
