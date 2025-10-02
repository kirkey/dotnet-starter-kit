using Accounting.Application.RecurringJournalEntries.Reactivate.v1;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntryReactivateEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntryReactivateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/reactivate", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new ReactivateRecurringJournalEntryCommand(id)).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(RecurringJournalEntryReactivateEndpoint))
            .WithSummary("Reactivate a recurring journal entry template")
            .WithDescription("Reactivate a suspended recurring journal entry template")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}
