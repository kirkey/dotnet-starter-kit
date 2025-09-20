using FSH.Starter.WebApi.Store.Application.Customers.Create.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

/// <summary>
/// Endpoint for creating new customers in the store system.
/// Supports retail, wholesale, and corporate customer creation with comprehensive validation.
/// </summary>
public static class CreateCustomerEndpoint
{
    /// <summary>
    /// Maps the create customer endpoint to handle POST requests for creating new customers.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>The configured route handler builder</returns>
    internal static RouteHandlerBuilder MapCreateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateCustomerCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateCustomerEndpoint))
            .WithSummary("Create a new customer")
            .WithDescription("Creates a new customer (retail, wholesale, or corporate) with comprehensive account information")
            .Produces<CreateCustomerResponse>()
            .RequirePermission("Permissions.Customers.Create")
            .MapToApiVersion(1);
    }
}
