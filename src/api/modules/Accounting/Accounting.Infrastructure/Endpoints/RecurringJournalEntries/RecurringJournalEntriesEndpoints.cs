using Accounting.Application.RecurringJournalEntries.Approve.v1;
using Accounting.Application.RecurringJournalEntries.Create.v1;
using Accounting.Application.RecurringJournalEntries.Delete.v1;
using Accounting.Application.RecurringJournalEntries.Generate.v1;
using Accounting.Application.RecurringJournalEntries.Get.v1;
using Accounting.Application.RecurringJournalEntries.Reactivate.v1;
using Accounting.Application.RecurringJournalEntries.Responses;
using Accounting.Application.RecurringJournalEntries.Search.v1;
using Accounting.Application.RecurringJournalEntries.Suspend.v1;
using Accounting.Application.RecurringJournalEntries.Update.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries;

/// <summary>
/// Endpoint configuration for Recurring Journal Entries module.
/// Provides comprehensive REST API endpoints for managing recurring journal entry templates.
/// </summary>
public class RecurringJournalEntriesEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Recurring Journal Entries endpoints to the route builder.
    /// Includes CRUD and workflow operations.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/recurring-journal-entries").WithTags("recurring-journal-entries");

        // Create endpoint
        group.MapPost("/", async (CreateRecurringJournalEntryCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/recurring-journal-entries/{response}", response);
            })
            .WithName("CreateRecurringJournalEntry")
            .WithSummary("Create a recurring journal entry template")
            .WithDescription("Create a new recurring journal entry template")
            .Produces<DefaultIdType>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting));

        // Get endpoint
        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetRecurringJournalEntryRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("GetRecurringJournalEntry")
            .WithSummary("Get a recurring journal entry template")
            .WithDescription("Get a recurring journal entry template by ID")
            .Produces<RecurringJournalEntryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Update endpoint
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateRecurringJournalEntryCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var entryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = entryId });
            })
            .WithName("UpdateRecurringJournalEntry")
            .WithSummary("Update recurring journal entry")
            .WithDescription("Updates a recurring journal entry template")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Delete endpoint
        group.MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteRecurringJournalEntryCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName("DeleteRecurringJournalEntry")
            .WithSummary("Delete a recurring journal entry template")
            .WithDescription("Delete a recurring journal entry template by ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting));

        // Search endpoint
        group.MapPost("/search", async (SearchRecurringJournalEntriesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName("SearchRecurringJournalEntries")
            .WithSummary("Search recurring journal entry templates")
            .WithDescription("Search and filter recurring journal entry templates with pagination")
            .Produces<PagedList<RecurringJournalEntryResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting));

        // Approve endpoint
        group.MapPost("/{id}/approve", async (DefaultIdType id, ApproveRecurringJournalEntryCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("ApproveRecurringJournalEntry")
            .WithSummary("Approve a recurring journal entry template")
            .WithDescription("Approve a recurring journal entry template for use")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting));

        // Suspend endpoint
        group.MapPost("/{id}/suspend", async (DefaultIdType id, SuspendRecurringJournalEntryCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("SuspendRecurringJournalEntry")
            .WithSummary("Suspend a recurring journal entry template")
            .WithDescription("Temporarily suspend a recurring journal entry template")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Reactivate endpoint
        group.MapPost("/{id}/reactivate", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new ReactivateRecurringJournalEntryCommand(id)).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName("ReactivateRecurringJournalEntry")
            .WithSummary("Reactivate a recurring journal entry template")
            .WithDescription("Reactivate a suspended recurring journal entry template")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting));

        // Generate endpoint
        group.MapPost("/{id:guid}/generate", async (DefaultIdType id, GenerateRecurringJournalEntryCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var journalEntryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { JournalEntryId = journalEntryId, Message = "Journal entry generated successfully" });
            })
            .WithName("GenerateRecurringJournalEntry")
            .WithSummary("Generate journal entry from template")
            .WithDescription("Manually generates a journal entry from a recurring template")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Generate, FshResources.Accounting));
    }
}
