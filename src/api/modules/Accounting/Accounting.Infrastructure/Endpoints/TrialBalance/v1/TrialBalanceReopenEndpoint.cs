using Accounting.Application.TrialBalance.Reopen.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TrialBalance.v1;

/// <summary>
/// Endpoint for reopening a finalized trial balance report.
/// </summary>
public static class TrialBalanceReopenEndpoint
{
    internal static RouteHandlerBuilder MapTrialBalanceReopenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reopen", async (DefaultIdType id, ReopenTrialBalanceCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var trialBalanceId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = trialBalanceId, Message = "Trial balance reopened successfully" });
            })
            .WithName(nameof(TrialBalanceReopenEndpoint))
            .WithSummary("Reopen a finalized trial balance")
            .WithDescription("Reopens a finalized trial balance to allow modifications")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
