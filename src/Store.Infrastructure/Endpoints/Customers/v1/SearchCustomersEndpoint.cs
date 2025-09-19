using FSH.Starter.WebApi.Store.Application.Customers.Get.v1;
using FSH.Starter.WebApi.Store.Application.Customers.Search.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

/// <summary>
/// Endpoint for searching customers with pagination and filtering capabilities.
/// </summary>
public static class SearchCustomersEndpoint
{
    /// <summary>
    /// Maps the search customers endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>Route handler builder for search customers endpoint</returns>
    internal static RouteHandlerBuilder MapSearchCustomersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchCustomersCommand command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchCustomersEndpoint))
            .WithSummary("Search customers")
            .WithDescription("Search and filter customers with pagination support")
            .Produces<PagedList<CustomerResponse>>()
            .RequirePermission("Permissions.Customers.View")
            .MapToApiVersion(1);
    }
}
