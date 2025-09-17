using Accounting.Application.Payments.Commands;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

public static class AllocatePaymentEndpoint
{
    internal static RouteHandlerBuilder MapAllocatePaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/allocate", async (AllocatePaymentCommand request, ISender mediator) =>
            {
                await mediator.Send(request).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(AllocatePaymentEndpoint))
            .WithSummary("Allocate a payment")
            .WithDescription("Allocate a payment to invoices")
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}


