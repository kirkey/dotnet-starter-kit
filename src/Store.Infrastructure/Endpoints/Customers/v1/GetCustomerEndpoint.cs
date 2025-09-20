using FSH.Starter.WebApi.Store.Application.Customers.Get.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class GetCustomerEndpoint
{
    internal static RouteHandlerBuilder MapGetCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var result = await mediator.Send(new GetCustomerRequest(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName(nameof(GetCustomerEndpoint))
        .WithSummary("Get customer by ID")
        .WithDescription("Retrieves a customer by their unique identifier")
        .Produces<CustomerResponse>()
        .RequirePermission("Permissions.Store.View")
        .MapToApiVersion(1);
    }
}
