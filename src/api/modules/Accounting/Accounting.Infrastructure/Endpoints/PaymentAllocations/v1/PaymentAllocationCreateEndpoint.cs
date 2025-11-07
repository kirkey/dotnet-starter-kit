using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;

/// <summary>
/// Endpoint for creating a new payment allocation.
/// </summary>
public static class PaymentAllocationCreateEndpoint
{
    /// <summary>
    /// Maps the payment allocation creation endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentAllocationCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePaymentAllocationCommand command, ISender mediator) =>
            {
                var id = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/payment-allocations/{id}", new { Id = id });
            })
            .WithName(nameof(PaymentAllocationCreateEndpoint))
            .WithSummary("Create a new payment allocation")
            .WithDescription("Allocates a payment amount to a specific invoice")
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}
