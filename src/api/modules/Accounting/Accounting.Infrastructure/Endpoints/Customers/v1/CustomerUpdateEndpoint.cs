using Accounting.Application.Customers.Update.v1;

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
            .MapPut("/{id}", async (DefaultIdType id, CustomerUpdateCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(CustomerUpdateEndpoint))
            .WithSummary("Update a customer")
            .WithDescription("Updates an existing customer's information.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

