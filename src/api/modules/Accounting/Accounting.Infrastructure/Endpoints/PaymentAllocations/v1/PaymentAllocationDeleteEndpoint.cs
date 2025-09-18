using Accounting.Application.PaymentAllocations.Commands;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;

public static class PaymentAllocationDeleteEndpoint
{
    internal static RouteHandlerBuilder MapPaymentAllocationDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeletePaymentAllocationCommand { Id = id }).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(PaymentAllocationDeleteEndpoint))
            .WithSummary("Deletes a payment allocation")
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(1);
    }
}

