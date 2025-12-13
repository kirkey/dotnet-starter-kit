using FSH.Starter.WebApi.Store.Application.GoodsReceipts.MarkReceived.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.GoodsReceipts.v1;

public static class MarkReceivedEndpoint
{
    internal static RouteHandlerBuilder MapMarkReceivedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/mark-received", async (DefaultIdType id, MarkReceivedCommand request, ISender sender) =>
            {
                if (id != request.GoodsReceiptId)
                {
                    return Results.BadRequest("Goods receipt ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkReceivedEndpoint))
            .WithSummary("Mark goods receipt as received")
            .WithDescription("Marks a goods receipt as received/completed.")
            .Produces<MarkReceivedResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);
    }
}
