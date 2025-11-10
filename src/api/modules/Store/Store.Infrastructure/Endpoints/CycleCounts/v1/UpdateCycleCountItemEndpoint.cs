using FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

/// <summary>
/// Endpoint for updating a cycle count item.
/// </summary>
public static class UpdateCycleCountItemEndpoint
{
    internal static RouteHandlerBuilder MapUpdateCycleCountItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/items/{id:guid}", async (DefaultIdType id, UpdateCycleCountItemCommand command, ISender sender) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(UpdateCycleCountItemEndpoint))
        .WithSummary("Update cycle count item")
        .WithDescription("Update the counted quantity and notes for a cycle count item")
        .Produces<DefaultIdType>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .MapToApiVersion(1);
    }
}

