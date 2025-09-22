using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Approve.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers.v1;

public static class ApproveInventoryTransferEndpoint
{
    internal static RouteHandlerBuilder MapApproveInventoryTransferEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveInventoryTransferCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ApproveInventoryTransferEndpoint))
        .WithSummary("Approve inventory transfer")
        .WithDescription("Approves an inventory transfer")
        .Produces<ApproveInventoryTransferResponse>()
        .RequirePermission("Permissions.InventoryTransfers.Approve")
        .MapToApiVersion(1);
    }
}

