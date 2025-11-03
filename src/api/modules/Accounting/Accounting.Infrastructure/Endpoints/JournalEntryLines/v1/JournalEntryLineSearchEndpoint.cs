using Accounting.Application.JournalEntries.Lines.Responses;
using Accounting.Application.JournalEntries.Lines.Search;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines.v1;

public static class JournalEntryLineSearchEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryLineSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("by-journal-entry/{journalEntryId:guid}", async (DefaultIdType journalEntryId, ISender mediator) =>
            {
                var list = await mediator.Send(new SearchJournalEntryLinesByJournalEntryIdQuery(journalEntryId));
                return Results.Ok(list);
            })
            .WithName(nameof(JournalEntryLineSearchEndpoint))
            .WithSummary("list journal entry lines by journal entry id")
            .WithDescription("retrieves all journal entry lines for a specific journal entry")
            .Produces<List<JournalEntryLineResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

