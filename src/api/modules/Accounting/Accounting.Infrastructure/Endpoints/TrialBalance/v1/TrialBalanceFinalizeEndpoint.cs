using Accounting.Application.TrialBalance.Finalize.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TrialBalance.v1;

/// <summary>
/// Endpoint for finalizing a trial balance report.
/// </summary>
public static class TrialBalanceFinalizeEndpoint
{
    internal static RouteHandlerBuilder MapTrialBalanceFinalizeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/finalize", async (DefaultIdType id, FinalizeTrialBalanceCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var trialBalanceId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = trialBalanceId, Message = "Trial balance finalized successfully" });
            })
            .WithName(nameof(TrialBalanceFinalizeEndpoint))
            .WithSummary("Finalize a trial balance")
            .WithDescription("Finalizes a trial balance report (validates balance and accounting equation)")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Complete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

