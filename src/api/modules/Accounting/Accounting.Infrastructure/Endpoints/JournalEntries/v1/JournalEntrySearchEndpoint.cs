using Accounting.Application.JournalEntries.Responses;
using Accounting.Application.JournalEntries.Search;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

public static class JournalEntrySearchEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntrySearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchJournalEntriesRequest request) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(JournalEntrySearchEndpoint))
            .WithSummary("Search journal entries")
            .WithDescription("Searches journal entries with pagination and filtering support")
            .Produces<PagedList<JournalEntryResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


