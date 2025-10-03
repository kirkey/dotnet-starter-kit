using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Delete.v1;

namespace Store.Infrastructure.Endpoints.GoodsReceipts.v1;

public static class DeleteGoodsReceiptEndpoint
{
    internal static RouteHandlerBuilder MapDeleteGoodsReceiptEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                var request = new DeleteGoodsReceiptCommand { GoodsReceiptId = id };
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteGoodsReceiptEndpoint))
            .WithSummary("Delete a goods receipt")
            .WithDescription("Deletes an existing goods receipt.")
            .Produces<DeleteGoodsReceiptResponse>()
            .RequirePermission("Permissions.Store.Delete")
            .MapToApiVersion(1);
    }
}
