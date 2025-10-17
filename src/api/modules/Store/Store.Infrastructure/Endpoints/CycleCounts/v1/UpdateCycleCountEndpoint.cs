using FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

namespace Store.Infrastructure.Endpoints.CycleCounts.v1;

/// <summary>
/// Endpoint for updating cycle count information.
/// Only cycle counts in 'Scheduled' status can be updated.
/// </summary>
public static class UpdateCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapUpdateCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateCycleCountCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest("ID in URL does not match ID in request body");
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateCycleCountEndpoint))
            .WithSummary("Update a cycle count")
            .WithDescription("Updates cycle count details. Only cycle counts in 'Scheduled' status can be updated.")
            .Produces<UpdateCycleCountResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .MapToApiVersion(1);
    }
}

