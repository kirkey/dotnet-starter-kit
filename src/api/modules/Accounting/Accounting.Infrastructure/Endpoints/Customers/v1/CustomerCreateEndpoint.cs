using Accounting.Application.Customers.Create;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerCreateEndpoint
{
    internal static RouteHandlerBuilder MapCustomerCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateCustomerRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerCreateEndpoint))
            .WithSummary("create a customer")
            .WithDescription("create a customer")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}


