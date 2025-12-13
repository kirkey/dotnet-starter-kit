using FSH.Starter.WebApi.Store.Application.StockAdjustments.Approve.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.StockAdjustments.v1;

public static class ApproveStockAdjustmentEndpoint
{
    internal static RouteHandlerBuilder MapApproveStockAdjustmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/approve", async (DefaultIdType id, ApproveStockAdjustmentCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ApproveStockAdjustmentEndpoint))
        .WithSummary("Approve stock adjustment")
        .WithDescription("Approves a stock adjustment and applies changes to inventory")
        .Produces<ApproveStockAdjustmentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Store))
        .MapToApiVersion(1);
    }
}
