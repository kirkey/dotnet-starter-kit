using Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries;

/// <summary>
/// Endpoint configuration for RecurringJournalEntries module.
/// Provides comprehensive REST API endpoints for managing recurring-journal-entries.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class RecurringJournalEntriesEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all RecurringJournalEntries endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/recurring-journal-entries").WithTags("recurring-journal-entrie");

        group.MapRecurringJournalEntryApproveEndpoint();
        group.MapRecurringJournalEntryCreateEndpoint();
        group.MapRecurringJournalEntryDeleteEndpoint();
        group.MapRecurringJournalEntryGenerateEndpoint();
        group.MapRecurringJournalEntryGetEndpoint();
        group.MapRecurringJournalEntryReactivateEndpoint();
        group.MapRecurringJournalEntrySearchEndpoint();
        group.MapRecurringJournalEntrySuspendEndpoint();
        group.MapRecurringJournalEntryUpdateEndpoint();
    }
}
