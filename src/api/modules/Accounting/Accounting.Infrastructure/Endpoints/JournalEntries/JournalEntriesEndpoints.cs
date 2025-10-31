using Accounting.Infrastructure.Endpoints.JournalEntries.v1;

namespace Accounting.Infrastructure.Endpoints.JournalEntries;

/// <summary>
/// Endpoint configuration for Journal Entries module.
/// </summary>
public static class JournalEntriesEndpoints
{
    /// <summary>
    /// Maps all Journal Entries endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapJournalEntriesEndpoints(this IEndpointRouteBuilder app)
    {
        var journalEntriesGroup = app.MapGroup("/journal-entries")
            .WithTags("Journal-Entries")
            .WithDescription("Endpoints for managing journal entries - the foundation of double-entry bookkeeping");

        // Version 1 endpoints - Query operations
        journalEntriesGroup.MapJournalEntryGetEndpoint();
        journalEntriesGroup.MapJournalEntrySearchEndpoint();

        return app;
    }
}

