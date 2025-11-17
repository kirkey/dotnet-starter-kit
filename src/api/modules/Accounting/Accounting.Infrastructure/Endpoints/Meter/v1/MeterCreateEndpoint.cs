using Accounting.Application.Meters.Create.v1;
using Shared.Authorization;

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
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

