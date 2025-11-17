using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Queries;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.GoodsReceipts.v1;

/// <summary>
/// Endpoint to get purchase order items available for receiving.
/// Shows ordered, received, and remaining quantities to support partial receiving.
/// </summary>
public static class GetPurchaseOrderItemsForReceivingEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseOrderItemsForReceivingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/purchase-order/{purchaseOrderId:guid}/items-for-receiving", async (
                DefaultIdType purchaseOrderId,
                ISender sender) =>
            {
                var query = new GetPurchaseOrderItemsForReceivingQuery
                {
                    PurchaseOrderId = purchaseOrderId
                };

                var response = await sender.Send(query);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPurchaseOrderItemsForReceivingEndpoint))
            .WithSummary("Get PO items available for receiving")
            .WithDescription("Returns purchase order items with their ordered, received, and remaining quantities for partial receiving support")
            .Produces<GetPurchaseOrderItemsForReceivingResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}

