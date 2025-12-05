using FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;
using FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;
using FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Taxes;

/// <summary>
/// Tax endpoints coordinator.
/// Maps all tax master configuration endpoints to their respective handlers.
/// </summary>
public class TaxEndpoints() : CarterModule("humanresources")
{
    /// <summary>
    /// Maps all tax endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("hr/taxes").WithTags("taxes");

        group.MapPost("/", async (CreateTaxCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute("CreateTax", new { id = response.Id }, response);
            })
            .WithName("CreateTax")
            .WithSummary("Create tax master configuration")
            .WithDescription("Creates a new tax master configuration for various tax types (VAT, GST, Excise, Withholding, Property, Sales Tax, etc.)")
            .Produces<CreateTaxResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Taxes))
            .MapToApiVersion(1);

        group.MapPut("/{id}", async (DefaultIdType id, UpdateTaxCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID does not match request ID");

                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("UpdateTax")
            .WithSummary("Update tax master configuration")
            .WithDescription("Updates an existing tax master configuration. Only provided fields are updated.")
            .Produces<DefaultIdType>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Taxes))
            .MapToApiVersion(1);

        group.MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetTaxRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetTax")
            .WithSummary("Get tax master configuration")
            .WithDescription("Retrieves a tax master configuration by ID with all details.")
            .Produces<TaxResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Taxes))
            .MapToApiVersion(1);

        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new DeleteTaxCommand(id);
                var result = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("DeleteTax")
            .WithSummary("Delete tax master configuration")
            .WithDescription("Deletes a tax master configuration by ID.")
            .Produces<DefaultIdType>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Taxes))
            .MapToApiVersion(1);

        group.MapPost("/search", async (SearchTaxesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchTaxes")
            .WithSummary("Search tax master configurations")
            .WithDescription("Searches and filters tax master configurations with pagination support. " +
                             "Supports filtering by code, tax type, jurisdiction, and active status.")
            .Produces<PagedList<TaxDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Taxes))
            .MapToApiVersion(1);
    }
}

