using FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;

namespace Store.Infrastructure.Endpoints.InventoryReservations.v1;

public static class SearchInventoryReservationsEndpoint
{
    internal static RouteHandlerBuilder MapSearchInventoryReservationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchInventoryReservationsRequest request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInventoryReservationsEndpoint))
            .WithSummary("Search inventory reservations")
            .WithDescription("Searches for inventory reservations with pagination and filtering by reservation number, item, warehouse, type, status, dates, and more.")
            .Produces<PagedList<InventoryReservationDto>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
