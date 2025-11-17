using FSH.Starter.WebApi.Store.Application.InventoryReservations.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryReservations.v1;

public static class GetInventoryReservationEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryReservationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var response = await sender.Send(new GetInventoryReservationCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetInventoryReservationEndpoint))
            .WithSummary("Get an inventory reservation by ID")
            .WithDescription("Retrieves a specific inventory reservation by its unique identifier.")
            .Produces<InventoryReservationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}
