using Accounting.Application.Customers.Dtos;
using Accounting.Application.Customers.Search;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerSearchEndpoint
{
    internal static RouteHandlerBuilder MapCustomerSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchCustomersQuery request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerSearchEndpoint))
            .WithSummary("search customers")
            .WithDescription("search customers")
            .Produces<PagedList<CustomerResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
