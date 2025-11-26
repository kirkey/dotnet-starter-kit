using Accounting.Application.Consumptions.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Consumptions.v1;

/// <summary>
/// Endpoint for updating a consumption record.
/// </summary>
public static class ConsumptionUpdateEndpoint
{
    internal static RouteHandlerBuilder MapConsumptionUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateConsumptionCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ConsumptionUpdateEndpoint))
            .WithSummary("Update consumption record")
            .WithDescription("Updates a consumption record")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

