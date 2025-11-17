using FSH.Starter.WebApi.Store.Application.InventoryTransfers.MarkInTransit.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class MarkInTransitInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapMarkInTransitInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/in-transit", async (DefaultIdType id, MarkInTransitInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MarkInTransitInventoryTransferEndpoint))
        .WithSummary("Mark inventory transfer as in-transit")
        .WithDescription("Marks an approved inventory transfer as InTransit")
        .Produces<MarkInTransitInventoryTransferResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
        .MapToApiVersion(1);
    }
}

