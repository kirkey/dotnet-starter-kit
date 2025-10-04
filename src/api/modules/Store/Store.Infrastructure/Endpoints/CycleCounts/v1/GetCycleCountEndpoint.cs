using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

public static class GetCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapGetCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCycleCountRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetCycleCountEndpoint))
        .WithSummary("Get cycle count by ID")
        .WithDescription("Retrieves a cycle count by its unique identifier")
        .Produces<CycleCountResponse>()
        .MapToApiVersion(1);
    }
}
