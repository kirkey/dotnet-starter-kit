using Carter;
using FSH.Starter.WebApi.Store.Application.Bins.Create.v1;
using FSH.Starter.WebApi.Store.Application.Bins.Delete.v1;
using FSH.Starter.WebApi.Store.Application.Bins.Get.v1;
using FSH.Starter.WebApi.Store.Application.Bins.Search.v1;
using FSH.Starter.WebApi.Store.Application.Bins.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.Bins;

/// <summary>
/// Endpoint configuration for Bins module.
/// </summary>
public class BinsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Bins endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/bins").WithTags("bins");

        group.MapPost("/", async (CreateBinCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/store/bins/{response.Id}", response);
            })
            .WithName("CreateBin")
            .WithSummary("Create a new bin")
            .WithDescription("Creates a new storage bin within a warehouse location")
            .Produces<CreateBinResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Warehouse));

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBinRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetBin")
            .WithSummary("Get bin by ID")
            .WithDescription("Retrieves a specific storage bin by its ID")
            .Produces<BinResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Warehouse));

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateBinCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateBin")
            .WithSummary("Update an existing bin")
            .WithDescription("Updates an existing storage bin")
            .Produces<UpdateBinResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse));

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var deletedId = await mediator.Send(new DeleteBinCommand(id)).ConfigureAwait(false);
                return Results.Ok(deletedId);
            })
            .WithName("DeleteBin")
            .WithSummary("Delete a bin")
            .WithDescription("Deletes a storage bin")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Warehouse));

        group.MapPost("/search", async (SearchBinsCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchBins")
            .WithSummary("Search bins")
            .WithDescription("Searches for storage bins with pagination and filtering")
            .Produces<PagedList<BinResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Warehouse));
    }
}
