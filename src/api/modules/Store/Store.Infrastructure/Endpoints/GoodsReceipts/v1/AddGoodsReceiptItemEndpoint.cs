using FSH.Starter.WebApi.Store.Application.GoodsReceipts.AddItem.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.GoodsReceipts.v1;

public static class AddGoodsReceiptItemEndpoint
{
    internal static RouteHandlerBuilder MapAddGoodsReceiptItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/items", async (DefaultIdType id, AddGoodsReceiptItemCommand request, ISender sender) =>
            {
                if (id != request.GoodsReceiptId)
                {
                    return Results.BadRequest("Goods receipt ID mismatch");
                }

                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AddGoodsReceiptItemEndpoint))
            .WithSummary("Add item to goods receipt")
            .WithDescription("Adds an item to an existing goods receipt.")
            .Produces<AddGoodsReceiptItemResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
