using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Approve.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for approving a submitted purchase order.
/// Moves order from Submitted to Approved status.
/// </summary>
public static class ApprovePurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the approve purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for approve purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapApprovePurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/approve", async (DefaultIdType id, ApprovePurchaseOrderRequest request, ISender mediator) =>
        {
            var command = new ApprovePurchaseOrderCommand(id, request.ApprovalNotes);
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(ApprovePurchaseOrderEndpoint))
        .WithSummary("Approve a submitted purchase order")
        .WithDescription("Approves a submitted purchase order, changing status from Submitted to Approved")
        .Produces<ApprovePurchaseOrderResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Store))
        .MapToApiVersion(1);
    }
}

/// <summary>
/// Request model for approve purchase order endpoint.
/// </summary>
public sealed record ApprovePurchaseOrderRequest(string? ApprovalNotes = null);
