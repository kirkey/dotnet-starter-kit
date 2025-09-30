using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Submit.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for submitting a purchase order for approval.
/// Moves order from Draft to Submitted status.
/// </summary>
public static class SubmitPurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the submit purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for submit purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapSubmitPurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/submit", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new SubmitPurchaseOrderCommand(id);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SubmitPurchaseOrderEndpoint))
        .WithSummary("Submit a purchase order for approval")
        .WithDescription("Submits a draft purchase order for approval, changing status from Draft to Submitted")
        .Produces<SubmitPurchaseOrderResponse>()
        .RequirePermission("Permissions.Store.Update")
        .MapToApiVersion(1);
    }
}
