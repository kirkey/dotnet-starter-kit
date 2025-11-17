using Accounting.Application.Checks.Issue.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for issuing a check for payment.
/// Transitions a check from Available to Issued status and records payment details.
/// </summary>
public static class CheckIssueEndpoint
{
    /// <summary>
    /// Maps the check issuance endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckIssueEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/issue", async (CheckIssueCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckIssueEndpoint))
            .WithSummary("Issue a check for payment")
            .WithDescription("Issue a check to a payee/vendor for payment of expenses, invoices, or other obligations. Transitions check to Issued status.")
            .Produces<CheckIssueResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

