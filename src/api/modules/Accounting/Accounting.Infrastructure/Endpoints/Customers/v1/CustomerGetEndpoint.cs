using Accounting.Application.Customers.Dtos;
using Accounting.Application.Customers.Get;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerGetEndpoint
{
    internal static RouteHandlerBuilder MapCustomerGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCustomerQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerGetEndpoint))
            .WithSummary("get a customer by id")
            .WithDescription("get a customer by id")
            .Produces<CustomerResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
