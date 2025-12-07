using Accounting.Infrastructure.Endpoints.JournalEntryLines.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.JournalEntryLines;

/// <summary>
/// Endpoint configuration for JournalEntryLines module.
/// Provides comprehensive REST API endpoints for managing journal-entry-lines.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class JournalEntryLinesEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all JournalEntryLines endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/journal-entry-lines").WithTags("journal-entry-line");

        group.MapJournalEntryLineCreateEndpoint();
        group.MapJournalEntryLineDeleteEndpoint();
        group.MapJournalEntryLineGetEndpoint();
        group.MapJournalEntryLineSearchEndpoint();
        group.MapJournalEntryLineUpdateEndpoint();
    }
}
