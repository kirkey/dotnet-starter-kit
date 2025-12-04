using Accounting.Application.FixedAssets.Approve.v1;
using Accounting.Application.FixedAssets.Create;
using Accounting.Application.FixedAssets.Delete;
using Accounting.Application.FixedAssets.Depreciate;
using Accounting.Application.FixedAssets.Dispose.v1;
using Accounting.Application.FixedAssets.Get;
using Accounting.Application.FixedAssets.Reject.v1;
using Accounting.Application.FixedAssets.Responses;
using Accounting.Application.FixedAssets.Search;
using Accounting.Application.FixedAssets.Update;
using Accounting.Application.FixedAssets.UpdateMaintenance.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.FixedAssets;

/// <summary>
/// Endpoint configuration for Fixed Assets module.
/// </summary>
public class FixedAssetsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Fixed Assets endpoints to the route builder.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/fixed-assets").WithTags("fixed-assets");

        // Create fixed asset
        group.MapPost("/", async (CreateFixedAssetCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/accounting/fixed-assets/{response.Id}", response);
            })
            .WithName("CreateFixedAsset")
            .WithSummary("create a fixed asset")
            .WithDescription("create a fixed asset")
            .Produces<CreateFixedAssetResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get fixed asset by ID
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetFixedAssetRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetFixedAsset")
            .WithSummary("get a fixed asset by id")
            .WithDescription("get a fixed asset by id")
            .Produces<FixedAssetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update fixed asset
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateFixedAssetCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("UpdateFixedAsset")
            .WithSummary("update a fixed asset")
            .WithDescription("update a fixed asset")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete fixed asset
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteFixedAssetRequest(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteFixedAsset")
            .WithSummary("delete fixed asset by id")
            .WithDescription("delete fixed asset by id")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search fixed assets
        group.MapPost("/search", async (ISender mediator, [FromBody] SearchFixedAssetsRequest command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchFixedAssets")
            .WithSummary("Gets a list of fixed assets")
            .WithDescription("Gets a list of fixed assets with pagination and filtering support")
            .Produces<PagedList<FixedAssetResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Depreciate fixed asset
        group.MapPost("/{id:guid}/depreciate", async (DefaultIdType id, DepreciateFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.FixedAssetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var assetId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = assetId, Message = "Depreciation recorded successfully" });
            })
            .WithName("DepreciateFixedAsset")
            .WithSummary("Record depreciation")
            .WithDescription("Records depreciation expense and reduces book value")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting));

        // Dispose fixed asset
        group.MapPost("/{id:guid}/dispose", async (DefaultIdType id, DisposeFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var assetId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = assetId, Message = "Asset disposed successfully" });
            })
            .WithName("DisposeFixedAsset")
            .WithSummary("Dispose asset")
            .WithDescription("Marks an asset as disposed and records disposal details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Update maintenance schedule
        group.MapPut("/{id:guid}/maintenance", async (DefaultIdType id, UpdateMaintenanceCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var assetId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = assetId, Message = "Maintenance schedule updated successfully" });
            })
            .WithName("UpdateFixedAssetMaintenance")
            .WithSummary("Update maintenance schedule")
            .WithDescription("Updates last and next maintenance dates")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Approve fixed asset
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.FixedAssetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("ApproveFixedAsset")
            .WithSummary("Approve fixed asset")
            .WithDescription("Approves a fixed asset for activation")
            .Produces<DefaultIdType>();

        // Reject fixed asset
        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectFixedAssetCommand command, ISender mediator) =>
            {
                if (id != command.FixedAssetId) return Results.BadRequest("ID in URL does not match ID in request body.");
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("RejectFixedAsset")
            .WithSummary("Reject fixed asset")
            .WithDescription("Rejects a fixed asset")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting));
    }
}
