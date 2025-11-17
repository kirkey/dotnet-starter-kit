using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Get.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.GoodsReceipts.v1;

public static class GetGoodsReceiptEndpoint
{
    internal static RouteHandlerBuilder MapGetGoodsReceiptEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var command = new GetGoodsReceiptCommand { GoodsReceiptId = id };
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetGoodsReceiptEndpoint))
            .WithSummary("Get goods receipt by ID")
            .WithDescription("Retrieves a specific goods receipt with all items.")
            .Produces<GetGoodsReceiptResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}
