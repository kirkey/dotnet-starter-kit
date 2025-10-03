using FSH.Starter.WebApi.Store.Application.InventoryReservations.Create.v1;

namespace Store.Infrastructure.Endpoints.InventoryReservations.v1;

public static class CreateInventoryReservationEndpoint
{
    internal static RouteHandlerBuilder MapCreateInventoryReservationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInventoryReservationCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInventoryReservationEndpoint))
            .WithSummary("Create a new inventory reservation")
            .WithDescription("Creates a new inventory reservation to prevent overselling and support order fulfillment.")
            .Produces<CreateInventoryReservationResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}
