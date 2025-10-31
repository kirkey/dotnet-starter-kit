using Accounting.Application.Customers.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

/// <summary>
/// Endpoint for creating new customers in the accounting system.
/// </summary>
public static class CustomerCreateEndpoint
{
    /// <summary>
    /// Maps the customer creation endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapCustomerCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CustomerCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/customers/{response.Id}", response);
            })
            .WithName(nameof(CustomerCreateEndpoint))
            .WithSummary("Create a new customer")
            .WithDescription("Creates a new customer account in the accounting system with comprehensive validation.")
            .Produces<CustomerCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

