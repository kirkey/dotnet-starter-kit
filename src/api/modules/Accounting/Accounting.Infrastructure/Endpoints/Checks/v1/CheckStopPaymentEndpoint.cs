using Accounting.Application.Checks.StopPayment.v1;

namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Endpoint for requesting stop payment on a check.
/// </summary>
public static class CheckStopPaymentEndpoint
{
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
            .WithDescription("Request stop payment on a check that is lost, stolen, or needs to be cancelled")
            .Produces<CheckStopPaymentResponse>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

