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
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PaymentUpdateEndpoint))
            .WithSummary("Update a payment")
            .WithDescription("Updates payment details (reference, deposit account, description, notes)")
            .Produces<PaymentUpdateResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}


