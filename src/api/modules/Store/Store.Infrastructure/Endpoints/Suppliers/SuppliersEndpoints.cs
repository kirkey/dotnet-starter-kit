using Carter;
using FSH.Starter.WebApi.Store.Application.Suppliers.Activate.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Deactivate.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Delete.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Get.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Search.v1;
using FSH.Starter.WebApi.Store.Application.Suppliers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Suppliers;

/// <summary>
/// Endpoint configuration for Suppliers module.
/// </summary>
public class SuppliersEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Suppliers endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/suppliers").WithTags("suppliers");

        // Create supplier
        group.MapPost("/", async (CreateSupplierCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateSupplierEndpoint")
        .WithSummary("Create a new supplier")
        .WithDescription("Creates a new supplier")
        .Produces<CreateSupplierResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Update supplier
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateSupplierCommand request, ISender sender) =>
        {
            var command = request with { Id = id };
            var result = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("UpdateSupplierEndpoint")
        .WithSummary("Update a supplier")
        .WithDescription("Updates an existing supplier")
        .Produces<UpdateSupplierResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Delete supplier
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteSupplierCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteSupplierEndpoint")
        .WithSummary("Delete a supplier")
        .WithDescription("Deletes a supplier by id")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        // Get supplier
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var result = await sender.Send(new GetSupplierCommand(id)).ConfigureAwait(false);
            return Results.Ok(result);
        })
        .WithName("GetSupplierEndpoint")
        .WithSummary("Get a supplier")
        .WithDescription("Retrieves a supplier by id")
        .Produces<SupplierResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Search suppliers
        group.MapPost("/search", async (ISender mediator, [FromBody] SearchSuppliersCommand command) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchSuppliersEndpoint")
        .WithSummary("Search Suppliers")
        .WithDescription("Searches Suppliers with pagination and filters")
        .Produces<PagedList<SupplierResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Activate supplier
        group.MapPost("/{id:guid}/activate", async (DefaultIdType id, ActivateSupplierCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Supplier ID mismatch");
            }
            
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("ActivateSupplierEndpoint")
        .WithSummary("Activate a supplier")
        .WithDescription("Activates a supplier to allow transactions")
        .Produces<ActivateSupplierResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        // Deactivate supplier
        group.MapPost("/{id:guid}/deactivate", async (DefaultIdType id, DeactivateSupplierCommand command, ISender sender) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("Supplier ID mismatch");
            }
            
            var response = await sender.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("DeactivateSupplierEndpoint")
        .WithSummary("Deactivate a supplier")
        .WithDescription("Deactivates a supplier to block transactions")
        .Produces<DeactivateSupplierResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);
    }
}
