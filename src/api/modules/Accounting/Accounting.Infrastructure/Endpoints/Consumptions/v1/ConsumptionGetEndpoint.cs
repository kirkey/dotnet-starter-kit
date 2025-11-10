using Accounting.Application.Consumptions.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Consumptions.v1;

/// <summary>
/// Endpoint for retrieving a consumption record by ID.
/// </summary>
public static class ConsumptionGetEndpoint
{
    internal static RouteGroupBuilder MapConsumptionGetEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetConsumptionRequest(id);
            var result = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ConsumptionGetEndpoint))
        .WithSummary("Get consumption record")
        .WithDescription("Retrieves a consumption record by ID")
        .RequirePermission("Permissions.Accounting.View")
        .MapToApiVersion(1);

        return group;
    }
}

