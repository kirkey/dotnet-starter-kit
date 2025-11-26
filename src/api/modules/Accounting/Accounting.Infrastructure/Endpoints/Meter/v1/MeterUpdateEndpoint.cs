using Accounting.Application.Meters.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Meter.v1;

/// <summary>
/// Endpoint for updating a meter.
/// </summary>
public static class MeterUpdateEndpoint
{
    internal static RouteHandlerBuilder MapMeterUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateMeterCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(MeterUpdateEndpoint))
            .WithSummary("Update meter")
            .WithDescription("Updates a meter")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

