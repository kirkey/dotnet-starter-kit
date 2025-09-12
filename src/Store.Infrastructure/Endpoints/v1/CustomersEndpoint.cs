using FSH.Starter.WebApi.Store.Application.Customers.Create.v1;
using FSH.Starter.WebApi.Store.Application.Customers.Delete.v1;
using FSH.Starter.WebApi.Store.Application.Customers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Store.Infrastructure.Endpoints.v1;

public class CustomersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("customers").WithTags("Customers");

        group.MapPost("/", async (CreateCustomerCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/customers/{result.Id}", result);
        })
        .WithName("CreateCustomer")
        .WithSummary("Create a new customer")
        .WithDescription("Creates a new customer (retail, wholesale, or corporate)");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetCustomerQuery(id));
            return Results.Ok(result);
        })
        .WithName("GetCustomer")
        .WithSummary("Get customer by ID")
        .WithDescription("Retrieves a customer by their unique identifier");

        group.MapPut("/{id:guid}", async (Guid id, UpdateCustomerCommand command, ISender sender) =>
        {
            if (id != command.Id)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateCustomer")
        .WithSummary("Update customer")
        .WithDescription("Updates an existing customer");

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteCustomerCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteCustomer")
        .WithSummary("Delete customer")
        .WithDescription("Deletes a customer from the system");

        group.MapPost("/search", async (SearchCustomersQuery query, ISender sender) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("SearchCustomers")
        .WithSummary("Search customers")
        .WithDescription("Search customers with filtering and pagination");

        group.MapPut("/{id:guid}/balance", async (Guid id, UpdateCustomerBalanceCommand command, ISender sender) =>
        {
            if (id != command.CustomerId)
                return Results.BadRequest("ID mismatch");

            var result = await sender.Send(command);
            return Results.Ok(result);
        })
        .WithName("UpdateCustomerBalance")
        .WithSummary("Update customer balance")
        .WithDescription("Updates customer account balance");
    }
}
