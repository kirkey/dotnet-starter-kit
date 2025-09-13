using FSH.Starter.WebApi.Store.Application.Customers.Delete.v1;

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
