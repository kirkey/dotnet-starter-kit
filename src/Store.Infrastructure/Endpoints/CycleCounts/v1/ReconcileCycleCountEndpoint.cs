using FSH.Starter.WebApi.Store.Application.CycleCounts.Reconcile.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class ReconcileCycleCountEndpoint
{
    public static RouteHandlerBuilder MapReconcileCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/reconcile", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new ReconcileCycleCountCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("ReconcileCycleCount")
        .WithSummary("Reconcile a cycle count")
        .WithDescription("Runs reconciliation for a completed cycle count and returns any discrepancies")
        .Produces<ReconcileCycleCountResponse>()
        .MapToApiVersion(1);
    }
}
