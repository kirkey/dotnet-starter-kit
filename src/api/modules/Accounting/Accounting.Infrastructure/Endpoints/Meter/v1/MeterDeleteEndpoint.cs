using Accounting.Application.Meters.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for deleting a meter.
/// </summary>
public static class MeterDeleteEndpoint
{
    internal static RouteGroupBuilder MapMeterDeleteEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeleteMeterCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MeterDeleteEndpoint))
        .WithSummary("Delete meter")
        .WithDescription("Deletes a meter (cannot have reading history)")
        .RequirePermission("Permissions.Accounting.Delete")
        .MapToApiVersion(1);

        return group;
    }
}

