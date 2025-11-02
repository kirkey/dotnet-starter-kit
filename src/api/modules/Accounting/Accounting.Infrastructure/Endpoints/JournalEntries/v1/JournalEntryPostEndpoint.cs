using Accounting.Application.JournalEntries.Post;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for posting a journal entry to the general ledger.
/// </summary>
public static class JournalEntryPostEndpoint
{
    /// <summary>
    /// Maps the post journal entry endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapJournalEntryPostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/post", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new PostJournalEntryCommand(id);
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = result, Message = "Journal entry posted successfully" });
            })
            .WithName(nameof(JournalEntryPostEndpoint))
            .WithSummary("Post a journal entry to the general ledger")
            .WithDescription("Post a balanced journal entry to the general ledger. The entry must be balanced (debits = credits) and cannot be modified after posting.")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Post")
            .MapToApiVersion(1);
    }
}

