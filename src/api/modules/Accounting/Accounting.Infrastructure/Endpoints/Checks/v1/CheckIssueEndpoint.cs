using Accounting.Application.Checks.Issue.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for issuing a check for payment.
/// </summary>
public static class CheckIssueEndpoint
{
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
            .WithDescription("Issue a check to a payee/vendor for payment of expenses or invoices")
            .Produces<CheckIssueResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

