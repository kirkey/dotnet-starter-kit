using Accounting.Application.PostingBatches.Approve.v1;
using Accounting.Application.PostingBatches.Create.v1;
using Accounting.Application.PostingBatches.Delete.v1;
using Accounting.Application.PostingBatches.Get.v1;
using Accounting.Application.PostingBatches.Post.v1;
using Accounting.Application.PostingBatches.Reject.v1;
using Accounting.Application.PostingBatches.Responses;
using Accounting.Application.PostingBatches.Reverse.v1;
using Accounting.Application.PostingBatches.Search.v1;
using Accounting.Application.PostingBatches.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PostingBatch;

/// <summary>
/// Endpoint configuration for Posting Batch module.
/// Provides comprehensive REST API endpoints for managing posting batches.
/// </summary>
public class PostingBatchEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Posting Batch endpoints to the route builder.
    /// Includes Create, Read, Update, Delete, Search, and workflow operations for posting batches.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/posting-batches").WithTags("posting-batches");

        // Create endpoint
        group.MapPost("/", async (PostingBatchCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/posting-batches/{response.Id}", response);
            })
            .WithName("CreatePostingBatch")
            .WithSummary("Create posting batch")
            .WithDescription("Creates a new posting batch")
            .Produces<PostingBatchCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new PostingBatchGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetPostingBatch")
            .WithSummary("Get posting batch by ID")
            .WithDescription("Retrieves a posting batch by its unique identifier")
            .Produces<PostingBatchGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Update endpoint
        group.MapPut("/{id}", async (DefaultIdType id, UpdatePostingBatchCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID in URL does not match ID in request body");

                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("UpdatePostingBatch")
            .WithSummary("Update posting batch")
            .WithDescription("Updates a draft or pending posting batch")
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Delete endpoint
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeletePostingBatchCommand(id);
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName("DeletePostingBatch")
            .WithSummary("Delete posting batch")
            .WithDescription("Deletes a draft or pending posting batch with no journal entries")
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);

        // Search endpoint
        group.MapPost("/search", async (PostingBatchSearchQuery request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchPostingBatches")
            .WithSummary("Search posting batches")
            .WithDescription("Searches posting batches with filtering and pagination")
            .Produces<PagedList<PostingBatchResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);

        // Approve endpoint
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, PostingBatchApproveCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var batchId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch approved successfully" });
            })
            .WithName("ApprovePostingBatch")
            .WithSummary("Approve a posting batch")
            .WithDescription("Approves a posting batch for posting")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Reject endpoint
        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, PostingBatchRejectCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var batchId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch rejected successfully" });
            })
            .WithName("RejectPostingBatch")
            .WithSummary("Reject a posting batch")
            .WithDescription("Rejects a posting batch")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
            .MapToApiVersion(1);

        // Post endpoint
        group.MapPost("/{id:guid}/post", async (DefaultIdType id, ISender mediator) =>
            {
                var batchId = await mediator.Send(new PostingBatchPostCommand(id)).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch posted successfully" });
            })
            .WithName("PostPostingBatch")
            .WithSummary("Post a posting batch")
            .WithDescription("Posts all journal entries in an approved batch")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);

        // Reverse endpoint
        group.MapPost("/{id:guid}/reverse", async (DefaultIdType id, PostingBatchReverseCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var batchId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = batchId, Message = "Posting batch reversed successfully" });
            })
            .WithName("ReversePostingBatch")
            .WithSummary("Reverse a posting batch")
            .WithDescription("Reverses all entries in a posted batch")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
