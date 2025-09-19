using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;

public static class PaymentAllocationGetEndpoint
{
    internal static RouteHandlerBuilder MapPaymentAllocationGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPaymentAllocationByIdQuery { Id = id }).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PaymentAllocationGetEndpoint))
            .WithSummary("Gets a payment allocation by id")
            .Produces<PaymentAllocationResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

