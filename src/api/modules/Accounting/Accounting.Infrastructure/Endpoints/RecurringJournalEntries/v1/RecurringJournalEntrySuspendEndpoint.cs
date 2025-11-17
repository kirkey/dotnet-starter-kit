using Accounting.Application.RecurringJournalEntries.Suspend.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntrySuspendEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntrySuspendEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/suspend", async (DefaultIdType id, SuspendRecurringJournalEntryCommand command, ISender mediator) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch");

                await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok();
            })
            .WithName(nameof(RecurringJournalEntrySuspendEndpoint))
            .WithSummary("Suspend a recurring journal entry template")
            .WithDescription("Temporarily suspend a recurring journal entry template")
            .Produces(StatusCodes.Status200OK)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
