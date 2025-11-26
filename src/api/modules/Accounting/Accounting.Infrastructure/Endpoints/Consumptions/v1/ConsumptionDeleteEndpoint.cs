using Accounting.Application.Consumptions.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Consumptions.v1;

/// <summary>
/// Endpoint for deleting a consumption record.
/// </summary>
public static class ConsumptionDeleteEndpoint
{
    internal static RouteHandlerBuilder MapConsumptionDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteConsumptionCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ConsumptionDeleteEndpoint))
            .WithSummary("Delete consumption record")
            .WithDescription("Deletes a consumption record")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

