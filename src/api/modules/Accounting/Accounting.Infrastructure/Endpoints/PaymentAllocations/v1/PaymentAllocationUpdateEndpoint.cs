using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;

/// <summary>
/// Endpoint for updating a payment allocation.
/// </summary>
public static class PaymentAllocationUpdateEndpoint
{
    /// <summary>
    /// Maps the payment allocation update endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentAllocationUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdatePaymentAllocationCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                {
                    return Results.BadRequest("ID in URL does not match ID in request body.");
                }

                var allocationId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = allocationId });
            })
            .WithName(nameof(PaymentAllocationUpdateEndpoint))
            .WithSummary("Update a payment allocation")
            .WithDescription("Updates the amount and/or notes of a payment allocation")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
