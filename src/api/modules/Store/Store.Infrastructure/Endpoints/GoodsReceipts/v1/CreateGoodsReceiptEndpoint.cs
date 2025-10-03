using FSH.Starter.WebApi.Store.Application.GoodsReceipts.Create.v1;

namespace Store.Infrastructure.Endpoints.GoodsReceipts.v1;

public static class CreateGoodsReceiptEndpoint
{
    internal static RouteHandlerBuilder MapCreateGoodsReceiptEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateGoodsReceiptCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateGoodsReceiptEndpoint))
            .WithSummary("Create a new goods receipt")
            .WithDescription("Creates a new goods receipt for tracking inbound deliveries from suppliers.")
            .Produces<CreateGoodsReceiptResponse>()
            .RequirePermission("Permissions.Store.Create")
            .MapToApiVersion(1);
    }
}
