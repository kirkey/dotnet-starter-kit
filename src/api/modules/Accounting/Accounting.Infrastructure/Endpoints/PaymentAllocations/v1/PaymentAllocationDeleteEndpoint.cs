using Accounting.Application.PaymentAllocations.Commands;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;

public static class PaymentAllocationDeleteEndpoint
{
    internal static RouteHandlerBuilder MapPaymentAllocationDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeletePaymentAllocationCommand(id)).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(PaymentAllocationDeleteEndpoint))
            .WithSummary("Deletes a payment allocation")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

