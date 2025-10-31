using Accounting.Application.PurchaseOrders.Responses;
using Accounting.Application.PurchaseOrders.Search.v1;

namespace Accounting.Infrastructure.Endpoints.PurchaseOrders.v1;

public static class PurchaseOrderSearchEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseOrderSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPurchaseOrdersRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PurchaseOrderSearchEndpoint))
            .WithSummary("Search purchase orders")
            .Produces<List<PurchaseOrderResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


