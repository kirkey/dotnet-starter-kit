using Accounting.Infrastructure.Endpoints.JournalEntries.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.JournalEntries;

/// <summary>
/// Endpoint configuration for JournalEntries module.
/// Provides comprehensive REST API endpoints for managing journal-entries.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class JournalEntriesEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all JournalEntries endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/journal-entries").WithTags("journal-entrie");

        group.MapJournalEntryApproveEndpoint();
        group.MapJournalEntryCreateEndpoint();
        group.MapJournalEntryDeleteEndpoint();
        group.MapJournalEntryGetEndpoint();
        group.MapJournalEntryPostEndpoint();
        group.MapJournalEntryRejectEndpoint();
        group.MapJournalEntryReverseEndpoint();
        group.MapJournalEntrySearchEndpoint();
        group.MapJournalEntryUpdateEndpoint();
    }
}
