using Accounting.Application.Checks.Void.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for voiding a check.
/// Transitions a check to Void status and records the reason for cancellation.
/// </summary>
public static class CheckVoidEndpoint
{
    /// <summary>
    /// Maps the check void endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckVoidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/void", async (VoidCheckCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckVoidEndpoint))
            .WithSummary("Void a check")
            .WithDescription("Void a check that was issued in error or needs to be cancelled. Records the void reason in audit trail.")
            .Produces<CheckVoidResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

