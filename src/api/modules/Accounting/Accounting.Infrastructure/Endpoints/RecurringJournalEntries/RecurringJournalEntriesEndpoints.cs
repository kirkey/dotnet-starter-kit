using Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries;

/// <summary>
/// Endpoint configuration for Recurring Journal Entries module.
/// </summary>
public static class RecurringJournalEntriesEndpoints
{
    /// <summary>
    /// Maps all Recurring Journal Entries endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapRecurringJournalEntriesEndpoints(this IEndpointRouteBuilder app)
    {
        var recurringGroup = app.MapGroup("/recurring-journal-entries")
            .WithTags("Recurring-Journal-Entries")
            .WithDescription("Endpoints for managing recurring journal entry templates");

        // CRUD operations
        recurringGroup.MapRecurringJournalEntryCreateEndpoint();
        recurringGroup.MapRecurringJournalEntryGetEndpoint();
        recurringGroup.MapRecurringJournalEntryUpdateEndpoint();
        recurringGroup.MapRecurringJournalEntryDeleteEndpoint();
        recurringGroup.MapRecurringJournalEntrySearchEndpoint();
        
        // Workflow operations
        recurringGroup.MapRecurringJournalEntryApproveEndpoint();
        recurringGroup.MapRecurringJournalEntrySuspendEndpoint();
        recurringGroup.MapRecurringJournalEntryReactivateEndpoint();
        recurringGroup.MapRecurringJournalEntryGenerateEndpoint();

        return app;
    }
}
