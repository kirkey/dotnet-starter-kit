using Accounting.Application.WriteOffs.Approve.v1;
using Accounting.Application.WriteOffs.Create.v1;
using Accounting.Application.WriteOffs.Get;
using Accounting.Application.WriteOffs.Post.v1;
using Accounting.Application.WriteOffs.RecordRecovery.v1;
using Accounting.Application.WriteOffs.Reject.v1;
using Accounting.Application.WriteOffs.Responses;
using Accounting.Application.WriteOffs.Reverse.v1;
using Accounting.Application.WriteOffs.Search.v1;
using Accounting.Application.WriteOffs.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.WriteOffs;

public class WriteOffsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/write-offs").WithTags("write-offs");

        // CRUD operations
        group.MapPost("/", async (WriteOffCreateCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Created($"/accounting/write-offs/{response.Id}", response);
        })
        .WithName("CreateWriteOff")
        .WithSummary("Create write-off")
        .Produces<WriteOffCreateResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetWriteOffRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetWriteOff")
        .WithSummary("Get write-off by ID")
        .WithDescription("Retrieves a write-off by its unique identifier")
        .Produces<WriteOffResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateWriteOffCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var writeOffId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = writeOffId });
        })
        .WithName("UpdateWriteOff")
        .WithSummary("Update write-off")
        .WithDescription("Updates a pending write-off details")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (SearchWriteOffsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchWriteOffs")
        .WithSummary("Search write-offs")
        .Produces<PagedList<WriteOffResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Workflow operations
        group.MapPost("/{id:guid}/approve", async (DefaultIdType id, ApproveWriteOffCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var writeOffId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = writeOffId, Message = "Write-off approved successfully" });
        })
        .WithName("ApproveWriteOff")
        .WithSummary("Approve write-off")
        .WithDescription("Approves a pending write-off for posting")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reject", async (DefaultIdType id, RejectWriteOffCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var writeOffId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = writeOffId, Message = "Write-off rejected successfully" });
        })
        .WithName("RejectWriteOff")
        .WithSummary("Reject write-off")
        .WithDescription("Rejects a pending write-off")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/post", async (DefaultIdType id, PostWriteOffCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var writeOffId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = writeOffId, Message = "Write-off posted to general ledger successfully" });
        })
        .WithName("PostWriteOff")
        .WithSummary("Post write-off to GL")
        .WithDescription("Posts an approved write-off to the general ledger")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/recovery", async (DefaultIdType id, RecordRecoveryCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var writeOffId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = writeOffId, Message = "Recovery recorded successfully" });
        })
        .WithName("RecordWriteOffRecovery")
        .WithSummary("Record recovery")
        .WithDescription("Records recovery of a previously written-off amount")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id:guid}/reverse", async (DefaultIdType id, ReverseWriteOffCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var writeOffId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = writeOffId, Message = "Write-off reversed successfully" });
        })
        .WithName("ReverseWriteOff")
        .WithSummary("Reverse write-off")
        .WithDescription("Reverses a posted write-off")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}

