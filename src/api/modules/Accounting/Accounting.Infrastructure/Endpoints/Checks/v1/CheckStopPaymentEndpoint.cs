using Accounting.Application.Checks.StopPayment.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for requesting stop payment on a check.
/// Transitions a check to StopPayment status and records the stop payment reason.
/// </summary>
public static class CheckStopPaymentEndpoint
{
    /// <summary>
    /// Maps the stop payment endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapCheckStopPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/stop-payment", async (CheckStopPaymentCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CheckStopPaymentEndpoint))
            .WithSummary("Request stop payment on check")
            .WithDescription("Request stop payment on a check that is lost, stolen, or needs to be cancelled before presentation to the bank.")
            .Produces<CheckStopPaymentResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

