using Store.Infrastructure.Endpoints.GoodsReceipts.v1;

namespace Store.Infrastructure.Endpoints.GoodsReceipts;

/// <summary>
/// Endpoint configuration for Goods Receipts module.
/// </summary>
public static class GoodsReceiptsEndpoints
{
    /// <summary>
    /// Maps all Goods Receipts endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapGoodsReceiptsEndpoints(this IEndpointRouteBuilder app)
    {
        var goodsReceiptsGroup = app.MapGroup("/goodsreceipts")
            .WithTags("GoodsReceipts")
            .WithDescription("Endpoints for managing goods receipts");

        // Version 1 endpoints
        goodsReceiptsGroup.MapCreateGoodsReceiptEndpoint();
        goodsReceiptsGroup.MapAddGoodsReceiptItemEndpoint();
        goodsReceiptsGroup.MapMarkReceivedEndpoint();
        goodsReceiptsGroup.MapDeleteGoodsReceiptEndpoint();
        goodsReceiptsGroup.MapGetGoodsReceiptEndpoint();
        goodsReceiptsGroup.MapSearchGoodsReceiptsEndpoint();

        return app;
    }
}
