using FSH.Starter.WebApi.Store.Application.CycleCounts.Cancel.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

/// <summary>
/// Endpoint for cancelling a cycle count.
/// </summary>
public static class CancelCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapCancelCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/cancel", 
            async (DefaultIdType id, CancelCycleCountCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Route ID does not match command ID");
            }

            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(CancelCycleCountEndpoint))
        .WithSummary("Cancel a cycle count")
        .WithDescription("Cancels a cycle count that is in 'Scheduled' or 'InProgress' status")
        .Produces<CancelCycleCountResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .MapToApiVersion(1);
    }
}

