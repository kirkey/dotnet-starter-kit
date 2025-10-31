using Accounting.Application.PurchaseOrders.Get;
using Accounting.Application.PurchaseOrders.Responses;

namespace Accounting.Infrastructure.Endpoints.PurchaseOrders.v1;

public static class PurchaseOrderGetEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseOrderGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPurchaseOrderRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PurchaseOrderGetEndpoint))
            .WithSummary("Get purchase order by ID")
            .Produces<PurchaseOrderResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

