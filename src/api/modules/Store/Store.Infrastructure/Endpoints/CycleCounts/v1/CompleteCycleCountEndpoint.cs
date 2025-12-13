using FSH.Starter.WebApi.Store.Application.CycleCounts.Complete.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class CompleteCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapCompleteCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/complete", async (DefaultIdType id, CompleteCycleCountCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Cycle count ID mismatch");
            }
            
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(CompleteCycleCountEndpoint))
        .WithSummary("Complete a cycle count")
        .WithDescription("Marks an in-progress cycle count as completed and computes metrics")
        .Produces<CompleteCycleCountResponse>()
        .MapToApiVersion(1);
    }
}
