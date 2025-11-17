using Accounting.Application.RecurringJournalEntries.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntryDeleteEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntryDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteRecurringJournalEntryCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(RecurringJournalEntryDeleteEndpoint))
            .WithSummary("Delete a recurring journal entry template")
            .WithDescription("Delete a recurring journal entry template by ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
