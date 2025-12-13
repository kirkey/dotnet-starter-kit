using Accounting.Application.Customers.Get;
using Accounting.Application.Customers.Queries;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

/// <summary>
/// Endpoint for retrieving a single customer by ID.
/// </summary>
public static class CustomerGetEndpoint
{
    /// <summary>
    /// Maps the customer get endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapCustomerGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCustomerRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerGetEndpoint))
            .WithSummary("Get customer by ID")
            .WithDescription("Retrieves detailed information about a specific customer.")
            .Produces<CustomerDetailsDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

