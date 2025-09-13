using FSH.Starter.WebApi.Store.Application.Customers.Create.v1;

namespace Store.Infrastructure.Endpoints.Customers.v1;

public static class CreateCustomerEndpoint
{
    internal static RouteHandlerBuilder MapCreateCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateCustomerCommand command, ISender sender) =>
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
