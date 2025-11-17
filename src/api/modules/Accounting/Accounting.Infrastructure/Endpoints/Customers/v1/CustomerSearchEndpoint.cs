using Accounting.Application.Customers.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

/// <summary>
/// Endpoint for searching and listing customers with pagination.
/// </summary>
public static class CustomerSearchEndpoint
{
    /// <summary>
    /// Maps the customer search endpoint to the route builder.
    /// Supports paginated search with filtering.
    /// </summary>
    internal static RouteHandlerBuilder MapCustomerSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (CustomerSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerSearchEndpoint))
            .WithSummary("Search customers with pagination")
            .WithDescription("Searches and lists customers with optional filters and pagination support.")
            .Produces<PagedList<CustomerSearchResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
