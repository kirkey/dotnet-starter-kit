using FSH.Starter.WebApi.Store.Application.SalesImports.Create.v1;
using FSH.Starter.WebApi.Store.Application.SalesImports.Get.v1;
using FSH.Starter.WebApi.Store.Application.SalesImports.Reverse.v1;
using FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.SalesImports;

/// <summary>
/// Endpoint configuration for Sales Imports module.
/// </summary>
public class SalesImportsEndpoints() : CarterModule("store")
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/sales-imports").WithTags("sales-imports");

        // Create sales import
        group.MapPost("/", async (CreateSalesImportCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("CreateSalesImport")
        .WithSummary("Import POS sales data")
        .WithDescription("Creates and processes a sales import from POS CSV file to update inventory levels")
        .Produces<CreateSalesImportResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Store))
        .MapToApiVersion(1);

        // Get sales import by ID
        group.MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetSalesImportRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetSalesImport")
        .WithSummary("Get sales import details")
        .WithDescription("Retrieves detailed information about a sales import including all items")
        .Produces<SalesImportDetailResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Search sales imports
        group.MapPost("/search", async (SearchSalesImportsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchSalesImports")
        .WithSummary("Search sales imports")
        .WithDescription("Searches sales imports with filtering and pagination")
        .Produces<PagedList<SalesImportResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
        .MapToApiVersion(1);

        // Reverse sales import
        group.MapPost("/{id:guid}/reverse", async (Guid id, ReverseSalesImportCommand request, ISender mediator) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("ID in URL does not match ID in request body");
            }

            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("ReverseSalesImport")
        .WithSummary("Reverse a sales import")
        .WithDescription("Reverses a completed sales import by creating offsetting inventory transactions")
        .Produces<DefaultIdType>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Store))
        .MapToApiVersion(1);
    }
}

