using Accounting.Application.JournalEntries.Reject;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for rejecting a journal entry.
/// </summary>
public static class JournalEntryRejectEndpoint
{
    /// <summary>
    /// Maps the reject journal entry endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapJournalEntryRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reject", async (DefaultIdType id, RejectJournalEntryRequest request, ISender mediator) =>
            {
                var command = new RejectJournalEntryCommand(id, request.RejectionReason);
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = result, Message = "Journal entry rejected" });
            })
            .WithName(nameof(JournalEntryRejectEndpoint))
            .WithSummary("Reject a journal entry")
            .WithDescription("Reject a pending journal entry. Rejected entries cannot be posted and may require correction.")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(1);
    }
}

/// <summary>
/// Request for rejecting a journal entry.
/// </summary>
public record RejectJournalEntryRequest(string RejectedBy, string? RejectionReason);
