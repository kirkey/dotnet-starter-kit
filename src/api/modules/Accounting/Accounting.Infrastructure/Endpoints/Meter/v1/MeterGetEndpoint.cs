using Accounting.Application.Meters.Get.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for retrieving a meter by ID.
/// </summary>
public static class MeterGetEndpoint
{
    internal static RouteGroupBuilder MapMeterGetEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetMeterRequest(id);
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MeterGetEndpoint))
        .WithSummary("Get meter")
        .WithDescription("Retrieves a meter by ID")
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

