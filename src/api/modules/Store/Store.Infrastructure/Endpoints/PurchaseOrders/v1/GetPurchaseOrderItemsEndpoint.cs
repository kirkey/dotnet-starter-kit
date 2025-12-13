using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Items.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for retrieving all items for a purchase order.
/// </summary>
internal static class GetPurchaseOrderItemsEndpoint
{
    /// <summary>
    /// Maps the get purchase order items endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGetPurchaseOrderItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}/items", async (DefaultIdType id, ISender sender) =>
        {
            var query = new GetPurchaseOrderItemsQuery(id);
            var response = await sender.Send(query).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetPurchaseOrderItemsEndpoint))
        .WithSummary("Get all items for a purchase order")
        .WithDescription("Retrieves all line items associated with a purchase order, including grocery item details.")
        .Produces<List<PurchaseOrderItemResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}
