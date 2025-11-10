using Accounting.Application.CostCenters.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

/// <summary>
/// Endpoint for deleting a cost center.
/// </summary>
public static class CostCenterDeleteEndpoint
{
    internal static RouteGroupBuilder MapCostCenterDeleteEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeleteCostCenterCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(CostCenterDeleteEndpoint))
        .WithSummary("Delete cost center")
        .WithDescription("Deletes an inactive cost center with no transactions")
        .RequirePermission("Permissions.Accounting.Delete")
        .MapToApiVersion(1);

        return group;
    }
}

