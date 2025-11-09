using Accounting.Application.RecurringJournalEntries.Responses;
using Accounting.Application.RecurringJournalEntries.Search.v1;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntrySearchEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntrySearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchRecurringJournalEntriesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RecurringJournalEntrySearchEndpoint))
            .WithSummary("Search recurring journal entry templates")
            .WithDescription("Search and filter recurring journal entry templates with pagination")
            .Produces<PagedList<RecurringJournalEntryResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
