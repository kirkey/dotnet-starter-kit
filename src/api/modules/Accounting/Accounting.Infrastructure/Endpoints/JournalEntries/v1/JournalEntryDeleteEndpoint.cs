using Accounting.Application.JournalEntries.Delete;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for deleting an unposted journal entry.
/// </summary>
public static class JournalEntryDeleteEndpoint
{
    /// <summary>
    /// Maps the delete journal entry endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapJournalEntryDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteJournalEntryCommand(id);
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(JournalEntryDeleteEndpoint))
            .WithSummary("Delete an unposted journal entry")
            .WithDescription("Delete a journal entry that has not been posted. Posted entries cannot be deleted - use reverse instead.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

