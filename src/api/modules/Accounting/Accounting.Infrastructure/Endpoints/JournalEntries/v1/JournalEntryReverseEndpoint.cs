using Accounting.Application.JournalEntries.Reverse;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for reversing a posted journal entry.
/// </summary>
public static class JournalEntryReverseEndpoint
{
    /// <summary>
    /// Maps the reverse journal entry endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapJournalEntryReverseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reverse", async (DefaultIdType id, ReverseJournalEntryRequest request, ISender mediator) =>
            {
                var command = new ReverseJournalEntryCommand(id, request.ReversalDate, request.ReversalReason);
                var reversingEntryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new ReverseJournalEntryResponse(reversingEntryId));
            })
            .WithName(nameof(JournalEntryReverseEndpoint))
            .WithSummary("Reverse a posted journal entry")
            .WithDescription("Create a reversing entry with opposite debit/credit amounts to correct a posted journal entry.")
            .Produces<ReverseJournalEntryResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

/// <summary>
/// Request for reversing a journal entry.
/// </summary>
public record ReverseJournalEntryRequest(DateTime ReversalDate, string ReversalReason);

