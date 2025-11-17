using Accounting.Application.Consumptions.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Consumptions.v1;

/// <summary>
/// Endpoint for deleting a consumption record.
/// </summary>
public static class ConsumptionDeleteEndpoint
{
    internal static RouteGroupBuilder MapConsumptionDeleteEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeleteConsumptionCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ConsumptionDeleteEndpoint))
        .WithSummary("Delete consumption record")
        .WithDescription("Deletes a consumption record")
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

