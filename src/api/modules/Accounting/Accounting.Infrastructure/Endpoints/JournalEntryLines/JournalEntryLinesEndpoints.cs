using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;
using Accounting.Application.JournalEntries.Lines.Create;
using Accounting.Application.JournalEntries.Lines.Get;
using Accounting.Application.JournalEntries.Lines.Responses;
using Accounting.Application.JournalEntries.Lines.Update;
using Accounting.Application.JournalEntries.Lines.Delete;
using Accounting.Application.JournalEntries.Lines.Search;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines;

/// <summary>
/// Endpoint configuration for Journal Entry Lines module.
/// </summary>
public class JournalEntryLinesEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/journal-entry-lines").WithTags("journal-entry-lines");

        group.MapPost(string.Empty, async (CreateJournalEntryLineCommand request, ISender mediator) =>
        {
            var id = await mediator.Send(request);
            return Results.Ok(id);
        })
        .WithName("CreateJournalEntryLine")
        .WithSummary("create journal entry line")
        .WithDescription("creates a new journal entry line")
        .Produces<DefaultIdType>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetJournalEntryLineQuery(id));
            return Results.Ok(response);
        })
        .WithName("GetJournalEntryLine")
        .WithSummary("get journal entry line by id")
        .WithDescription("gets journal entry line by id")
        .Produces<JournalEntryLineResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateJournalEntryLineCommand request, ISender mediator) =>
        {
            if (id != request.Id)
                return Results.BadRequest("ID mismatch");

            await mediator.Send(request);
            return Results.NoContent();
        })
        .WithName("UpdateJournalEntryLine")
        .WithSummary("update journal entry line")
        .WithDescription("updates journal entry line")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteJournalEntryLineCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteJournalEntryLine")
        .WithSummary("delete journal entry line")
        .WithDescription("deletes journal entry line")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);

        group.MapGet("by-journal-entry/{journalEntryId:guid}", async (DefaultIdType journalEntryId, ISender mediator) =>
        {
            var list = await mediator.Send(new SearchJournalEntryLinesByJournalEntryIdQuery(journalEntryId));
            return Results.Ok(list);
        })
        .WithName("SearchJournalEntryLines")
        .WithSummary("list journal entry lines by journal entry id")
        .WithDescription("retrieves all journal entry lines for a specific journal entry")
        .Produces<List<JournalEntryLineResponse>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}
