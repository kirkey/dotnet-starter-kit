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

        // Version 1 endpoints
        recurringGroup.MapRecurringJournalEntryCreateEndpoint();
        recurringGroup.MapRecurringJournalEntryGetEndpoint();
        recurringGroup.MapRecurringJournalEntryDeleteEndpoint();
        recurringGroup.MapRecurringJournalEntrySearchEndpoint();
        recurringGroup.MapRecurringJournalEntryApproveEndpoint();
        recurringGroup.MapRecurringJournalEntrySuspendEndpoint();
        recurringGroup.MapRecurringJournalEntryReactivateEndpoint();

        return app;
    }
}
