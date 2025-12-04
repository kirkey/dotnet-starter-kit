using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using Accounting.Application.JournalEntries.Create;
using Accounting.Application.JournalEntries.Get;
using Accounting.Application.JournalEntries.Responses;
using Accounting.Application.JournalEntries.Update;
using Accounting.Application.JournalEntries.Delete;
using Accounting.Application.JournalEntries.Search;
using Accounting.Application.JournalEntries.Post;
using Accounting.Application.JournalEntries.Reverse;
using Accounting.Application.JournalEntries.Approve;
using Accounting.Application.JournalEntries.Reject;

namespace Accounting.Infrastructure.Endpoints.JournalEntries;

/// <summary>
/// Request for reversing a journal entry.
/// </summary>
public record ReverseJournalEntryRequest(DateTime ReversalDate, string ReversalReason);

/// <summary>
/// Request for rejecting a journal entry.
/// </summary>
public record RejectJournalEntryRequest(string RejectedBy, string? RejectionReason);

/// <summary>
/// Endpoint configuration for Journal Entries module.
/// </summary>
public class JournalEntriesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/journal-entries").WithTags("journal-entries");

        // Query operations
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var request = new GetJournalEntryRequest(id);
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("GetJournalEntry")
        .WithSummary("Get journal entry by ID")
        .WithDescription("Retrieve a specific journal entry by its identifier")
        .Produces<JournalEntryResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/search", async (ISender mediator, [FromBody] SearchJournalEntriesRequest request) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("SearchJournalEntries")
        .WithSummary("Search journal entries")
        .WithDescription("Searches journal entries with pagination and filtering support")
        .Produces<PagedList<JournalEntryResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        // Command operations
        group.MapPost("/", async (CreateJournalEntryCommand command, ISender mediator) =>
        {
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.CreatedAtRoute("GetJournalEntry", new { id = response.Id }, response);
        })
        .WithName("CreateJournalEntry")
        .WithSummary("Create a new journal entry")
        .WithDescription("Create a new journal entry with balanced debit and credit lines for double-entry accounting.")
        .Produces<CreateJournalEntryResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateJournalEntryCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName("UpdateJournalEntry")
        .WithSummary("Update an unposted journal entry")
        .WithDescription("Update the details of an unposted journal entry. Posted entries cannot be modified - use reverse instead.")
        .Produces<UpdateJournalEntryResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteJournalEntryCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName("DeleteJournalEntry")
        .WithSummary("Delete an unposted journal entry")
        .WithDescription("Delete a journal entry that has not been posted. Posted entries cannot be deleted - use reverse instead.")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);

        // Workflow operations
        group.MapPost("/{id}/post", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new PostJournalEntryCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = result, Message = "Journal entry posted successfully" });
        })
        .WithName("PostJournalEntry")
        .WithSummary("Post a journal entry to the general ledger")
        .WithDescription("Post a balanced journal entry to the general ledger. The entry must be balanced (debits = credits) and cannot be modified after posting.")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id}/reverse", async (DefaultIdType id, ReverseJournalEntryRequest request, ISender mediator) =>
        {
            var command = new ReverseJournalEntryCommand(id, request.ReversalDate, request.ReversalReason);
            var reversingEntryId = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new ReverseJournalEntryResponse(reversingEntryId));
        })
        .WithName("ReverseJournalEntry")
        .WithSummary("Reverse a posted journal entry")
        .WithDescription("Create a reversing entry with opposite debit/credit amounts to correct a posted journal entry.")
        .Produces<ReverseJournalEntryResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id}/approve", async (DefaultIdType id, ISender mediator) =>
        {
            var command = new ApproveJournalEntryCommand(id);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = result, Message = "Journal entry approved successfully" });
        })
        .WithName("ApproveJournalEntry")
        .WithSummary("Approve a journal entry")
        .WithDescription("Approve a pending journal entry. Approved entries can then be posted to the general ledger. The approver is automatically determined from the current user session.")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPost("/{id}/reject", async (DefaultIdType id, RejectJournalEntryRequest request, ISender mediator) =>
        {
            var command = new RejectJournalEntryCommand(id, request.RejectionReason);
            var result = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(new { Id = result, Message = "Journal entry rejected" });
        })
        .WithName("RejectJournalEntry")
        .WithSummary("Reject a journal entry")
        .WithDescription("Reject a pending journal entry. Rejected entries cannot be posted and may require correction.")
        .Produces<object>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}

