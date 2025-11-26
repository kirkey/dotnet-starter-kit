using Accounting.Application.Meters.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for deleting a meter.
/// </summary>
public static class MeterDeleteEndpoint
{
    internal static RouteHandlerBuilder MapMeterDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteMeterCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(MeterDeleteEndpoint))
            .WithSummary("Delete meter")
            .WithDescription("Deletes a meter (cannot have reading history)")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

