using Accounting.Application.RecurringJournalEntries.Approve.v1;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntryApproveEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntryApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/approve", async (DefaultIdType id, ApproveRecurringJournalEntryCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(RecurringJournalEntryApproveEndpoint))
            .WithSummary("Approve a recurring journal entry template")
            .WithDescription("Approve a recurring journal entry template for use")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Approve")
            .MapToApiVersion(1);
    }
}
