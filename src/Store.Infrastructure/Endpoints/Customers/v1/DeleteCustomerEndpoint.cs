using Store.Application.Customers.Delete.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class DeleteCustomerEndpoint
{
    internal static RouteHandlerBuilder MapDeleteCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteCustomerCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteCustomer")
        .WithSummary("Delete customer")
        .WithDescription("Deletes a customer by their unique identifier")
        .MapToApiVersion(1);
    }
}
using Store.Application.Customers.Get.v1;
using MediatR;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class GetCustomerEndpoint
{
    internal static RouteHandlerBuilder MapGetCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/customers/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerQuery(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetCustomer")
        .WithSummary("Get customer by ID")
        .WithDescription("Retrieves a customer by their unique identifier")
        .MapToApiVersion(1);
    }
}
