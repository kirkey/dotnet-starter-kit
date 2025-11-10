using Accounting.Application.Meters.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for searching meters.
/// </summary>
public static class MeterSearchEndpoint
{
    internal static RouteGroupBuilder MapMeterSearchEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/search", async (SearchMetersRequest request, ISender mediator) =>
        {
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(MeterSearchEndpoint))
        .WithSummary("Search meters")
        .WithDescription("Search meters with filters and pagination")
        .RequirePermission("Permissions.Accounting.View")
        .MapToApiVersion(1);

        return group;
    }
}

