using Accounting.Application.ChartOfAccounts.Deactivate.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

/// <summary>
/// Endpoint for deactivating a chart of account.
/// </summary>
public static class ChartOfAccountDeactivateEndpoint
{
    internal static RouteGroupBuilder MapChartOfAccountDeactivateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id}/deactivate", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new DeactivateChartOfAccountCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ChartOfAccountDeactivateEndpoint))
        .WithSummary("Deactivate chart of account")
        .WithDescription("Deactivates a chart of account")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}

