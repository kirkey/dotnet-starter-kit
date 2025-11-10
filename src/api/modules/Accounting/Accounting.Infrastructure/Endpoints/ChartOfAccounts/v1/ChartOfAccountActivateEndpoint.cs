using Accounting.Application.ChartOfAccounts.Activate.v1;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

/// <summary>
/// Endpoint for activating a chart of account.
/// </summary>
public static class ChartOfAccountActivateEndpoint
{
    internal static RouteGroupBuilder MapChartOfAccountActivateEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id}/activate", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new ActivateChartOfAccountCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ChartOfAccountActivateEndpoint))
        .WithSummary("Activate chart of account")
        .WithDescription("Activates a chart of account")
        .RequirePermission("Permissions.Accounting.Update")
        .MapToApiVersion(1);

        return group;
    }
}

