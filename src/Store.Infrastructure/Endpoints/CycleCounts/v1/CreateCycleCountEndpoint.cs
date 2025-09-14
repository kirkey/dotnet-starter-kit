using FSH.Starter.WebApi.Store.Application.CycleCounts.Create.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class CreateCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapCreateCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateCycleCountCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/cycle-counts/{result.Id}", result);
        })
        .WithName("CreateCycleCount")
        .WithSummary("Create a new cycle count")
        .WithDescription("Schedules a new cycle count for a warehouse or location")
        .MapToApiVersion(1);
    }
}

