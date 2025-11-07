using Accounting.Application.RecurringJournalEntries.Update.v1;

namespace Accounting.Infrastructure.Endpoints.RecurringJournalEntries.v1;

public static class RecurringJournalEntryUpdateEndpoint
{
    internal static RouteHandlerBuilder MapRecurringJournalEntryUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateRecurringJournalEntryCommand command, ISender mediator) =>
            {
                if (id != command.Id) return Results.BadRequest("ID in URL does not match ID in request body.");
                var entryId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = entryId });
            })
            .WithName(nameof(RecurringJournalEntryUpdateEndpoint))
            .WithSummary("Update recurring journal entry")
            .WithDescription("Updates a recurring journal entry template")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

