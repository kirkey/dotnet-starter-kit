using Accounting.Application.JournalEntries.Approve;

namespace Accounting.Infrastructure.Endpoints.JournalEntries.v1;

/// <summary>
/// Endpoint for approving a journal entry.
/// </summary>
public static class JournalEntryApproveEndpoint
{
    /// <summary>
    /// Maps the approve journal entry endpoint to the route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Route handler builder for further configuration.</returns>
    internal static RouteHandlerBuilder MapJournalEntryApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ApproveJournalEntryRequest request, ISender mediator) =>
            {
                var command = new ApproveJournalEntryCommand(id, request.ApprovedBy);
                var result = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = result, Message = "Journal entry approved successfully" });
            })
            .WithName(nameof(JournalEntryApproveEndpoint))
            .WithSummary("Approve a journal entry")
            .WithDescription("Approve a pending journal entry. Approved entries can then be posted to the general ledger.")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(1);
    }
}

/// <summary>
/// Request for approving a journal entry.
/// </summary>
public record ApproveJournalEntryRequest(string ApprovedBy);

