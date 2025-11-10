using FSH.Starter.WebApi.Store.Application.PurchaseOrders.GetItemsNeedingReorder.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for retrieving items that need reordering for a specific supplier.
/// Returns items at or below their reorder point with suggested order quantities.
/// </summary>
public static class GetItemsNeedingReorderEndpoint
{
    /// <summary>
    /// Maps the get items needing reorder endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for get items needing reorder endpoint</returns>
    internal static RouteHandlerBuilder MapGetItemsNeedingReorderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/suppliers/{supplierId:guid}/items-needing-reorder", 
            async (DefaultIdType supplierId, [FromBody] GetItemsNeedingReorderRequest request, ISender sender) =>
        {
            // Override supplier ID from route parameter
            var query = request with { SupplierId = supplierId };
            var result = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetItemsNeedingReorderEndpoint))
        .WithSummary("Get items needing reorder for a supplier")
        .WithDescription("Returns a list of items that are at or below their reorder point for the specified supplier, with suggested order quantities based on current stock levels")
        .Produces<List<ItemNeedingReorderResponse>>()
        .RequirePermission("Permissions.Store.View")
        .MapToApiVersion(1);
    }
}

