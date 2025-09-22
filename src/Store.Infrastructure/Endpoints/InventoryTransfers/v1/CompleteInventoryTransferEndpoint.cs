using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Complete.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class CompleteInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapCompleteInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/complete", async (DefaultIdType id, CompleteInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(CompleteInventoryTransferEndpoint))
        .WithSummary("Complete inventory transfer")
        .WithDescription("Marks an in-transit inventory transfer as completed and records actual arrival")
        .Produces<CompleteInventoryTransferResponse>()
        .RequirePermission("Permissions.InventoryTransfers.Update")
        .MapToApiVersion(1);
    }
}

