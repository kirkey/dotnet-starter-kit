using Carter;
using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Create.v1;
using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Delete.v1;
using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;
using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1;
using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using GetItemSupplierResponse = FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1.ItemSupplierResponse;
using SearchItemSupplierResponse = FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1.ItemSupplierResponse;

namespace Store.Infrastructure.Endpoints.ItemSuppliers;

public class ItemSuppliersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/item-suppliers").WithTags("item-suppliers");

        group.MapPost("/", async (CreateItemSupplierCommand request, ISender sender) =>
        {
            var response = await sender.Send(request);
            return Results.Created($"/api/v1/store/item-suppliers/{response.Id}", response);
        })
        .WithName("CreateItemSupplier")
        .WithSummary("Create a new item-supplier relationship")
        .WithDescription("Creates a new relationship between an item and a supplier with pricing and lead time details")
        .Produces<CreateItemSupplierResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateItemSupplierCommand request, ISender sender) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID in URL does not match ID in request body");
            }

            var response = await sender.Send(request);
            return Results.Ok(response);
        })
        .WithName("UpdateItemSupplier")
        .WithSummary("Update an item-supplier relationship")
        .WithDescription("Updates pricing, lead time, and other details for an existing item-supplier relationship")
        .Produces<UpdateItemSupplierResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            await sender.Send(new DeleteItemSupplierCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteItemSupplier")
        .WithSummary("Delete an item-supplier relationship")
        .WithDescription("Removes an item-supplier relationship from the system")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetItemSupplierCommand(id));
            return Results.Ok(response);
        })
        .WithName("GetItemSupplier")
        .WithSummary("Get an item-supplier relationship by ID")
        .WithDescription("Retrieves detailed information about a specific item-supplier relationship")
        .Produces<GetItemSupplierResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchItemSuppliersCommand command, ISender sender) =>
        {
            var response = await sender.Send(command);
            return Results.Ok(response);
        })
        .WithName("SearchItemSuppliers")
        .WithSummary("Search item-supplier relationships")
        .WithDescription("Search and filter item-supplier relationships with pagination support")
        .Produces<PagedList<SearchItemSupplierResponse>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);
    }
}
