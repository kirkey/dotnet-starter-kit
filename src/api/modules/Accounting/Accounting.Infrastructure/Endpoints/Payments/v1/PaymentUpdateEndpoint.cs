using Accounting.Application.Payments.Update.v1;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

/// <summary>
/// Endpoint for updating a payment.
/// </summary>
public static class PaymentUpdateEndpoint
{
    /// <summary>
    /// Maps the payment update endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, PaymentUpdateCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PaymentUpdateEndpoint))
            .WithSummary("Update a payment")
            .WithDescription("Updates payment details (reference, deposit account, description, notes)")
            .Produces<PaymentUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}


