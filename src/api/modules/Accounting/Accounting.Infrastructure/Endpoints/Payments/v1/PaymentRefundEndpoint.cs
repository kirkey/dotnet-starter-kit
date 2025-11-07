using Accounting.Application.Payments.Refund;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

/// <summary>
/// Endpoint for refunding a payment.
/// </summary>
public static class PaymentRefundEndpoint
{
    /// <summary>
    /// Maps the payment refund endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapRefundPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/refund", async (DefaultIdType id, RefundPaymentCommand request, ISender mediator) =>
            {
                if (id != request.PaymentId)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var paymentId = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(new { PaymentId = paymentId, Message = "Payment refunded successfully" });
            })
            .WithName(nameof(PaymentRefundEndpoint))
            .WithSummary("Refund a payment")
            .WithDescription("Issues a refund for a payment or partial payment amount")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

