using Accounting.Application.PaymentAllocations.Queries;
using Accounting.Application.PaymentAllocations.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;

public static class PaymentAllocationSearchEndpoint
{
    internal static RouteHandlerBuilder MapPaymentAllocationSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async ([FromBody] SearchPaymentAllocationsQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PaymentAllocationSearchEndpoint))
            .WithSummary("Searches payment allocations")
            .Produces<List<PaymentAllocationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

