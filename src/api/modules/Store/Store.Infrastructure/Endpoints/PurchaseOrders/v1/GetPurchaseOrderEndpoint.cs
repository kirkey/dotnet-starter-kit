using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Get.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for retrieving a purchase order by ID.
/// </summary>
public static class GetPurchaseOrderEndpoint
{
    /// <summary>
    /// Maps the get purchase order endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for get purchase order endpoint</returns>
    internal static RouteHandlerBuilder MapGetPurchaseOrderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetPurchaseOrderQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetPurchaseOrderEndpoint))
        .WithSummary("Get a purchase order")
        .WithDescription("Retrieves a purchase order by ID")
        .Produces<PurchaseOrderResponse>()
        .RequirePermission("Permissions.Store.View")
        .MapToApiVersion(1);
    }
}
