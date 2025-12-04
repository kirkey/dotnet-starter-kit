using Carter;
using FSH.Framework.Core.Storage.Commands;
using FSH.Framework.Core.Storage.Queries;
using FSH.Starter.WebApi.Store.Application.Items.Create.v1;
using FSH.Starter.WebApi.Store.Application.Items.Delete.v1;
using FSH.Starter.WebApi.Store.Application.Items.Export.v1;
using FSH.Starter.WebApi.Store.Application.Items.Get.v1;
using FSH.Starter.WebApi.Store.Application.Items.Import.v1;
using FSH.Starter.WebApi.Store.Application.Items.Search.v1;
using FSH.Starter.WebApi.Store.Application.Items.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Items;

/// <summary>
/// Endpoint configuration for Items module.
/// </summary>
public class ItemsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Items endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/items").WithTags("items");

        group.MapPost("/", async (CreateItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/store/items/{response.Id}", response);
            })
            .WithName("CreateItem")
            .WithSummary("Create a new item")
            .WithDescription("Creates a new inventory item")
            .Produces<CreateItemResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetItemCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetItem")
            .WithSummary("Get item by ID")
            .WithDescription("Retrieves a specific inventory item by its ID")
            .Produces<ItemResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateItemCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateItem")
            .WithSummary("Update an existing item")
            .WithDescription("Updates an existing inventory item")
            .Produces<UpdateItemResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var deletedId = await mediator.Send(new DeleteItemCommand(id)).ConfigureAwait(false);
                return Results.Ok(deletedId);
            })
            .WithName("DeleteItem")
            .WithSummary("Delete an item")
            .WithDescription("Deletes an inventory item")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Store))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchItemsCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchItems")
            .WithSummary("Search items")
            .WithDescription("Searches for inventory items with pagination and filtering")
            .Produces<PagedList<ItemResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);

        group.MapPost("/import", async (ImportItemsCommand command, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("ImportItems")
            .WithSummary("Import items from Excel file")
            .WithDescription("Imports items from an Excel file with validation. Returns ImportResponse with successful/failed counts and detailed error messages.")
            .Produces<ImportResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Import, FshResources.Store))
            .MapToApiVersion(1);

        group.MapPost("/export", async (ExportItemsQuery query, ISender mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(query, cancellationToken).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("ExportItems")
            .WithSummary("Export items to Excel file")
            .WithDescription("Exports items to Excel format with optional filtering by category, supplier, price range, and other criteria. Returns an ExportResponse with file data.")
            .Produces<ExportResponse>()
            .Produces<ExportResponse>(400)
            .RequirePermission(FshPermission.NameFor(FshActions.Export, FshResources.Store))
            .MapToApiVersion(1);
    }
}
