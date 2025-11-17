using Accounting.Application.Checks.Clear.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for marking a check as cleared.
/// Transitions a check to Cleared status during bank reconciliation.
/// </summary>
public static class CheckClearEndpoint
{
    /// <summary>
    /// Maps the check clear endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckClearEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/clear", async (CheckClearCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckClearEndpoint))
            .WithSummary("Mark check as cleared")
            .WithDescription("Mark a check as cleared during bank reconciliation when it appears on the bank statement.")
            .Produces<CheckClearResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

