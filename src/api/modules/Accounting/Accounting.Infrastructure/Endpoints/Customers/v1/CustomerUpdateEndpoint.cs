using Accounting.Application.Customers.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

/// <summary>
/// Endpoint for updating existing customers.
/// </summary>
public static class CustomerUpdateEndpoint
{
    /// <summary>
    /// Maps the customer update endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapCustomerUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, CustomerUpdateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CustomerUpdateEndpoint))
            .WithSummary("Update a customer")
            .WithDescription("Updates an existing customer's information.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

