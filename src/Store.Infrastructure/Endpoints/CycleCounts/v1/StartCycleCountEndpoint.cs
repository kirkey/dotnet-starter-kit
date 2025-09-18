using FSH.Starter.WebApi.Store.Application.CycleCounts.Start.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class StartCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapStartCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/start", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new StartCycleCountCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("StartCycleCount")
        .WithSummary("Start a cycle count")
        .WithDescription("Marks a scheduled cycle count as in-progress")
        .Produces<StartCycleCountResponse>()
        .MapToApiVersion(1);
    }
}
