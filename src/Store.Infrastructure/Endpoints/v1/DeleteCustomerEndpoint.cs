namespace Store.Infrastructure.Endpoints.v1;

public static class DeleteCustomerEndpoint
{
    internal static RouteHandlerBuilder MapDeleteCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/customers/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new FSH.Starter.WebApi.Store.Application.Customers.Delete.v1.DeleteCustomerCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteCustomer")
        .WithSummary("Delete customer")
        .WithDescription("Deletes a customer by their unique identifier")
        .MapToApiVersion(1);
    }
}
namespace Store.Infrastructure.Endpoints.v1;

public static class CreateCustomerEndpoint
{
    internal static RouteHandlerBuilder MapCreateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/customers", async (FSH.Starter.WebApi.Store.Application.Customers.Create.v1.CreateCustomerCommand command, ISender sender) =>
        {
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Created($"/customers/{result.Id}", result);
        })
        .WithName("CreateCustomer")
        .WithSummary("Create a new customer")
        .WithDescription("Creates a new customer (retail, wholesale, or corporate)")
        .MapToApiVersion(1);
    }
}

