using Accounting.Application.Vendors.Create.v1;
using Accounting.Application.Vendors.Delete.v1;
using Accounting.Application.Vendors.Get.v1;
using Accounting.Application.Vendors.Search.v1;
using Accounting.Application.Vendors.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Vendors;

/// <summary>
/// Endpoint configuration for Vendors module.
/// Provides comprehensive REST API endpoints for managing vendor accounts.
/// </summary>
public class VendorsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/vendors").WithTags("vendors");

        group.MapPost("/", async (VendorCreateCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateVendor")
        .WithSummary("create a vendor")
        .WithDescription("create a vendor")
        .Produces<VendorCreateResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, VendorUpdateCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateVendor")
        .WithSummary("Update a vendor")
        .WithDescription("Updates an existing vendor")
        .Produces<VendorUpdateResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new VendorDeleteCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteVendor")
        .WithSummary("delete vendor by id")
        .WithDescription("delete vendor by id")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new VendorGetRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetVendor")
        .WithSummary("Get a vendor by ID")
        .WithDescription("Retrieves a vendor by its unique identifier")
        .Produces<VendorGetResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (ISender mediator, [FromBody] VendorSearchRequest request) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchVendors")
        .WithSummary("Search vendors")
        .WithDescription("Searches vendors with pagination and filtering support")
        .Produces<PagedList<VendorSearchResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}

