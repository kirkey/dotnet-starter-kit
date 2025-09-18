using Accounting.Application.PaymentAllocations.Queries;
using Microsoft.AspNetCore.Mvc;

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
            .Produces<List<Application.PaymentAllocations.Dtos.PaymentAllocationDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

