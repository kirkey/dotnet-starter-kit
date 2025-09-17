using Accounting.Application.Customers.Update;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerUpdateEndpoint
{
    internal static RouteHandlerBuilder MapCustomerUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateCustomerRequest request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerUpdateEndpoint))
            .WithSummary("update a customer")
            .WithDescription("update a customer")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}


