using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Delete.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for deleting a purchase order.
/// </summary>
public static class DeletePurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the delete purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for delete purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapDeletePurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeletePurchaseOrderCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(DeletePurchaseOrderEndpoint))
        .WithSummary("Delete a purchase order")
        .WithDescription("Deletes a purchase order by ID")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission("Permissions.Store.Delete")
        .MapToApiVersion(1);
    }
}
