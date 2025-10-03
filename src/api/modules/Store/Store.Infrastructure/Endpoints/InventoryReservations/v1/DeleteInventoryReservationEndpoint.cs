using FSH.Starter.WebApi.Store.Application.InventoryReservations.Delete.v1;

namespace Store.Infrastructure.Endpoints.InventoryReservations.v1;

public static class DeleteInventoryReservationEndpoint
{
    internal static RouteHandlerBuilder MapDeleteInventoryReservationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new DeleteInventoryReservationCommand { Id = id }).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteInventoryReservationEndpoint))
            .WithSummary("Delete an inventory reservation")
            .WithDescription("Deletes an existing inventory reservation from the system.")
            .Produces<DeleteInventoryReservationResponse>()
            .RequirePermission("Permissions.Store.Delete")
            .MapToApiVersion(1);
    }
}
