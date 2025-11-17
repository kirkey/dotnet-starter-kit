using Accounting.Application.ChartOfAccounts.UpdateBalance.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

/// <summary>
/// Endpoint for updating a chart of account balance.
/// </summary>
public static class ChartOfAccountUpdateBalanceEndpoint
{
    internal static RouteGroupBuilder MapChartOfAccountUpdateBalanceEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id}/balance", async (DefaultIdType id, UpdateChartOfAccountBalanceCommand command, ISender mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID in URL does not match ID in request body");

            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(ChartOfAccountUpdateBalanceEndpoint))
        .WithSummary("Update chart of account balance")
        .WithDescription("Updates a chart of account balance")
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        return group;
    }
}

