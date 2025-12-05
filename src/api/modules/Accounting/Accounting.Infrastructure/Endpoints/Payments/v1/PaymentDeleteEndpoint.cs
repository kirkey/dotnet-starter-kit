using Accounting.Application.Payments.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

/// <summary>
/// Endpoint for deleting a payment.
/// </summary>
public static class PaymentDeleteEndpoint
{
    /// <summary>
    /// Maps the payment deletion endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeletePaymentCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(PaymentDeleteEndpoint))
            .WithSummary("Delete a payment")
            .WithDescription("Deletes a payment. Cannot delete payments with allocations.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


