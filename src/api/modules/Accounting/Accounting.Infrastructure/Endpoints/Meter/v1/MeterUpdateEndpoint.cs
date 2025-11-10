using Accounting.Application.Meters.Update.v1;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for updating a meter.
/// </summary>
public static class MeterUpdateEndpoint
{
    internal static RouteGroupBuilder MapMeterUpdateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}", async (DefaultIdType id, UpdateMeterCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MeterUpdateEndpoint))
        .WithSummary("Update meter")
        .WithDescription("Updates a meter")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}

