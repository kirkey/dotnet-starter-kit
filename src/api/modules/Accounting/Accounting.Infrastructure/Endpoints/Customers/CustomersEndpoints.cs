using Accounting.Application.Customers.Create.v1;
using Accounting.Application.Customers.Get;
using Accounting.Application.Customers.Queries;
using Accounting.Application.Customers.Search.v1;
using Accounting.Application.Customers.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Customers;

/// <summary>
/// Endpoint configuration for Customers module.
/// Provides comprehensive REST API endpoints for managing customer accounts.
/// </summary>
public class CustomersEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Customers endpoints to the route builder.
    /// Includes Create, Read, Update, and Search operations for customers.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/customers").WithTags("customers");

        // Create endpoint
        group.MapPost("/", async (CustomerCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/customers/{response.Id}", response);
            })
            .WithName("CreateCustomer")
            .WithSummary("Create a new customer")
            .WithDescription("Creates a new customer account in the accounting system with comprehensive validation.")
            .Produces<CustomerCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCustomerRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetCustomer")
            .WithSummary("Get customer by ID")
            .WithDescription("Retrieves detailed information about a specific customer.")
            .Produces<CustomerDetailsDto>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id}", async (DefaultIdType id, CustomerUpdateCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("UpdateCustomer")
            .WithSummary("Update a customer")
            .WithDescription("Updates an existing customer's information.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (CustomerSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchCustomers")
            .WithSummary("Search customers with pagination")
            .WithDescription("Searches and lists customers with optional filters and pagination support.")
            .Produces<PagedList<CustomerSearchResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));
    }
}

