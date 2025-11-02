using Accounting.Infrastructure.Endpoints.JournalEntryLines.v1;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines;

/// <summary>
/// Endpoint configuration for Journal Entry Lines module.
/// </summary>
public static class JournalEntryLinesEndpoints
{
    /// <summary>
    /// Maps all Journal Entry Lines endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapJournalEntryLinesEndpoints(this IEndpointRouteBuilder app)
    {
        var journalEntryLinesGroup = app.MapGroup("/journal-entry-lines")
            .WithTags("Journal-Entry-Lines")
            .WithDescription("Endpoints for managing journal entry line items");

        // Version 1 endpoints
        journalEntryLinesGroup.MapJournalEntryLineCreateEndpoint();
        journalEntryLinesGroup.MapJournalEntryLineUpdateEndpoint();
        journalEntryLinesGroup.MapJournalEntryLineDeleteEndpoint();
        journalEntryLinesGroup.MapJournalEntryLineGetEndpoint();
        journalEntryLinesGroup.MapJournalEntryLineSearchEndpoint();

        return app;
    }
}
