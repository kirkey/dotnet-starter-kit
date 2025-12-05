using Accounting.Application.Payments.Void;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

/// <summary>
/// Endpoint for voiding a payment.
/// </summary>
public static class PaymentVoidEndpoint
{
    /// <summary>
    /// Maps the payment void endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentVoidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/void", async (DefaultIdType id, VoidPaymentCommand request, ISender mediator) =>
            {
                if (id != request.PaymentId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var paymentId = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(new { PaymentId = paymentId, Message = "Payment voided successfully" });
            })
            .WithName(nameof(PaymentVoidEndpoint))
            .WithSummary("Void a payment")
            .WithDescription("Voids a payment and reverses all allocations")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

