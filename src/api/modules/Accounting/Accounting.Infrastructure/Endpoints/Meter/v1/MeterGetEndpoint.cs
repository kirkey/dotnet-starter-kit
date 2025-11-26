using Accounting.Application.Meters.Get.v1;
using Accounting.Application.Meters.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for retrieving a meter by ID.
/// </summary>
public static class MeterGetEndpoint
{
    internal static RouteHandlerBuilder MapMeterGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetMeterRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(MeterGetEndpoint))
            .WithSummary("Get meter")
            .WithDescription("Retrieves a meter by ID")
            .Produces<MeterResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

