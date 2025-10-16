using FSH.Starter.WebApi.Store.Application.CycleCounts.RecordCount.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

/// <summary>
/// Endpoint for recording counted quantities for cycle count items.
/// </summary>
public static class RecordCycleCountItemEndpoint
{
    internal static RouteHandlerBuilder MapRecordCycleCountItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{cycleCountId:guid}/items/{itemId:guid}/record", 
            async (DefaultIdType cycleCountId, DefaultIdType itemId, RecordCycleCountItemCommand command, ISender sender) =>
        {
            // Validate route parameters match command
            if (cycleCountId != command.CycleCountId || itemId != command.CycleCountItemId)
            {
                return Results.BadRequest("Route parameters do not match command properties");
            }

            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(RecordCycleCountItemEndpoint))
        .WithSummary("Record counted quantity for a cycle count item")
        .WithDescription("Records the physically counted quantity for a specific item during the counting phase. This is the core operation of cycle counting.")
        .Produces<RecordCycleCountItemResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);
    }
}

