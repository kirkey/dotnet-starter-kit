using FSH.Starter.WebApi.Store.Application.InventoryReservations.Release.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryReservations.v1;

public static class ReleaseInventoryReservationEndpoint
{
    internal static RouteHandlerBuilder MapReleaseInventoryReservationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/release", async (DefaultIdType id, ReleaseInventoryReservationCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID mismatch between route and body.");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ReleaseInventoryReservationEndpoint))
            .WithSummary("Release an inventory reservation")
            .WithDescription("Releases an active inventory reservation, returning the quantity to available stock.")
            .Produces<ReleaseInventoryReservationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
