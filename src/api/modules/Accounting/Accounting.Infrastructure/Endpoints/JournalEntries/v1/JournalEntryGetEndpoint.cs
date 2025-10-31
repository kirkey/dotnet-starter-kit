using Accounting.Application.JournalEntries.Get;
using Accounting.Application.JournalEntries.Responses;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for retrieving a single journal entry by ID.
/// </summary>
public static class JournalEntryGetEndpoint
{
    /// <summary>
    /// Maps the journal entry get endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapJournalEntryGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var query = new GetJournalEntryQuery(id);
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(JournalEntryGetEndpoint))
            .WithSummary("Get journal entry by ID")
            .WithDescription("Retrieve a specific journal entry by its identifier")
            .Produces<JournalEntryResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

