using FSH.Starter.WebApi.Store.Application.CycleCounts.Complete.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class CompleteCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapCompleteCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/complete", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new CompleteCycleCountCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("CompleteCycleCount")
        .WithSummary("Complete a cycle count")
        .WithDescription("Marks an in-progress cycle count as completed and computes metrics")
        .MapToApiVersion(1);
    }
}

