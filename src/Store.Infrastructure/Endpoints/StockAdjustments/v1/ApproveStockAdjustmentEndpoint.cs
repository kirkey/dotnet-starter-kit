using FSH.Starter.WebApi.Store.Application.StockAdjustments.Approve.v1;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

public static class ApproveStockAdjustmentEndpoint
{
    internal static RouteHandlerBuilder MapApproveStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveStockAdjustmentCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ApproveStockAdjustmentEndpoint))
        .WithSummary("Approve stock adjustment")
        .WithDescription("Approves a stock adjustment and applies changes to inventory")
        .Produces<ApproveStockAdjustmentResponse>()
        .RequirePermission("Permissions.Store.Approve")
        .MapToApiVersion(1);
    }
}
