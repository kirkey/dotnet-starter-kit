using FSH.Starter.WebApi.Store.Application.InventoryTransactions.Approve.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransactions.v1;

public static class ApproveInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapApproveInventoryTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveInventoryTransactionCommand request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID mismatch between route and body.");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveInventoryTransactionEndpoint))
            .WithSummary("Approve an inventory transaction")
            .WithDescription("Approves a pending inventory transaction for authorization and compliance.")
            .Produces<ApproveInventoryTransactionResponse>()
            .RequirePermission("Permissions.Store.Update")
            .MapToApiVersion(1);
    }
}
