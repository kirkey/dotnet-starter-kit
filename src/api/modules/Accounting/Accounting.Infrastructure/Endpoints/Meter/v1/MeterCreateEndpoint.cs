using Accounting.Application.Meters.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for creating a new meter.
/// </summary>
public static class MeterCreateEndpoint
{
    internal static RouteGroupBuilder MapMeterCreateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (CreateMeterCommand command, ISender mediator) =>
        {
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MeterCreateEndpoint))
        .WithSummary("Create meter")
        .WithDescription("Creates a new meter")
        .RequirePermission("Permissions.Accounting.Create")
        .MapToApiVersion(1);

        return group;
    }
}

