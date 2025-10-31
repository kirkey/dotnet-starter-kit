using Accounting.Application.Customers.Queries;
using Accounting.Application.Customers.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

/// <summary>
/// Endpoint for searching and listing customers.
/// </summary>
public static class CustomerSearchEndpoint
{
    /// <summary>
    /// Maps the customer search endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapCustomerSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchCustomersRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerSearchEndpoint))
            .WithSummary("Search customers")
            .WithDescription("Searches and lists customers with optional filters.")
            .Produces<List<CustomerDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
