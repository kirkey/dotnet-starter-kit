using FSH.Starter.WebApi.Store.Application.CycleCounts.Start.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class StartCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapStartCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/start", async (DefaultIdType id, StartCycleCountCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Cycle count ID mismatch");
            }
            
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(StartCycleCountEndpoint))
        .WithSummary("Start a cycle count")
        .WithDescription("Marks a scheduled cycle count as in-progress")
        .Produces<StartCycleCountResponse>()
        .MapToApiVersion(1);
    }
}
