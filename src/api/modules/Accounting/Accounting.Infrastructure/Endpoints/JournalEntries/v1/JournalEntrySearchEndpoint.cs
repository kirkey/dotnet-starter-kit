using Accounting.Application.JournalEntries.Dtos;
using Accounting.Application.JournalEntries.Search;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

public static class JournalEntrySearchEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntrySearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchJournalEntriesQuery command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(JournalEntrySearchEndpoint))
            .WithSummary("Gets a list of journal entries")
            .WithDescription("Gets a list of journal entries with pagination and filtering support")
            .Produces<PagedList<JournalEntryDto>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


